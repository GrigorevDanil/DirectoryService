using CSharpFunctionalExtensions;
using DirectoryService.Application.Constants;
using DirectoryService.Application.Locations;
using DirectoryService.Domain.DepartmentLocations;
using DirectoryService.Domain.DepartmentLocations.ValueObjects;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Locations.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedService.Core.Caching;
using SharedService.Core.Database;
using SharedService.Core.Handlers;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Departments.UseCases.Create;

public class CreateDepartmentHandler : ICommandHandler<CreateDepartmentCommand, Guid>
{
    private readonly ILocationRepository _locationRepository;

    private readonly IDepartmentRepository _departmentRepository;

    private readonly IValidator<CreateDepartmentCommand> _validator;

    private readonly ILogger _logger;

    private readonly ITransactionManager _transactionManager;

    private readonly ICacheService _cache;

    public CreateDepartmentHandler(
        IValidator<CreateDepartmentCommand> validator,
        ILocationRepository locationRepository,
        IDepartmentRepository departmentRepository,
        ILogger<CreateDepartmentHandler> logger,
        ITransactionManager transactionManager,
        ICacheService cache)
    {
        _validator = validator;
        _locationRepository = locationRepository;
        _departmentRepository = departmentRepository;
        _logger = logger;
        _transactionManager = transactionManager;
        _cache = cache;
    }

    public async Task<Result<Guid, Errors>> Handle(
        CreateDepartmentCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrors();

        var departmentId = DepartmentId.Create();
        var name = DepartmentName.Of(command.Request.Name).Value;
        var identifier = Identifier.Of(command.Request.Identifier).Value;

        var checkExistingIdsResult = await _locationRepository.CheckExistingAndActiveIds(command.Request.LocationIds, cancellationToken);

        if (checkExistingIdsResult.IsFailure)
            return checkExistingIdsResult.Error;

        var locationsIds = command.Request.LocationIds
            .Select(locationId =>
                new DepartmentLocation(DepartmentLocationId.Create(), departmentId, LocationId.Of(locationId)));

        // Создать родительское подразделение
        if (command.Request.ParentId == null)
        {
            var createParentDepartmentResult = Department.CreateParent(name, identifier, locationsIds, departmentId);

            if (createParentDepartmentResult.IsFailure)
                return createParentDepartmentResult.Error.ToErrors();

            var parentDepartment = createParentDepartmentResult.Value;

            await _departmentRepository.AddDepartmentAsync(parentDepartment, cancellationToken);
        }

        // Создать дочернее подразделение
        else
        {
            var parentId = DepartmentId.Of(command.Request.ParentId.GetValueOrDefault());

            var getParentDepartmentResult = await _departmentRepository.GetActiveDepartmentByIdAsync(parentId, cancellationToken);

            if (getParentDepartmentResult.IsFailure)
                return getParentDepartmentResult.Error.ToErrors();

            var parentDepartment = getParentDepartmentResult.Value;

            var createChildDepartmentResult = Department.CreateChild(name,  identifier, parentDepartment, locationsIds, departmentId);

            if (createChildDepartmentResult.IsFailure)
                return createChildDepartmentResult.Error.ToErrors();

            var childDepartment = createChildDepartmentResult.Value;

            await _departmentRepository.AddDepartmentAsync(childDepartment, cancellationToken);
        }

        var commitedResult = await _transactionManager.SaveChangesAsyncWithResult(cancellationToken);

        if (commitedResult.IsFailure)
            return commitedResult.Error.ToErrors();

        await _cache.RemoveByPrefixAsync(CachingKeys.DEPARTMENTS_KEY, cancellationToken);

        _logger.LogInformation("Department by id {DepartmentId} has been added.", departmentId.Value);

        return departmentId.Value;
    }
}