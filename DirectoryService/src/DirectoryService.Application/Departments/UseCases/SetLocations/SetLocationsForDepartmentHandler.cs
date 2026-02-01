using CSharpFunctionalExtensions;
using DirectoryService.Application.Constants;
using DirectoryService.Application.Locations;
using DirectoryService.Domain.DepartmentLocations;
using DirectoryService.Domain.DepartmentLocations.ValueObjects;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Locations.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedService.Core.Caching;
using SharedService.Core.Database;
using SharedService.Core.Handlers;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Departments.UseCases.SetLocations;

public class SetLocationsForDepartmentHandler : ICommandHandler<SetLocationsForDepartmentCommand, Guid>
{
    private readonly IDepartmentRepository _departmentRepository;

    private readonly ILocationRepository _locationRepository;

    private readonly IValidator<SetLocationsForDepartmentCommand> _validator;

    private readonly ILogger<SetLocationsForDepartmentHandler> _logger;

    private readonly ITransactionManager _transactionManager;

    private readonly ICacheService _cache;

    public SetLocationsForDepartmentHandler(
        IDepartmentRepository departmentRepository,
        IValidator<SetLocationsForDepartmentCommand> validator,
        ILogger<SetLocationsForDepartmentHandler> logger,
        ITransactionManager transactionManager,
        ILocationRepository locationRepository,
        ICacheService cache)
    {
        _departmentRepository = departmentRepository;
        _validator = validator;
        _logger = logger;
        _transactionManager = transactionManager;
        _locationRepository = locationRepository;
        _cache = cache;
    }

    public async Task<Result<Guid, Errors>> Handle(
        SetLocationsForDepartmentCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrors();

        var departmentId = DepartmentId.Of(command.DepartmentId);

        var getDepartmentResult = await _departmentRepository.GetByAsync(x => x.Id == departmentId && x.IsActive == true, cancellationToken);

        if (getDepartmentResult.IsFailure)
            return getDepartmentResult.Error.ToErrors();

        var department = getDepartmentResult.Value;

        var checkExistingIdsResult = await _locationRepository.CheckExistingAndActiveIds(command.ForDepartmentRequest.LocationIds, cancellationToken);

        if (checkExistingIdsResult.IsFailure)
            return checkExistingIdsResult.Error;

        var locationsIds = command.ForDepartmentRequest.LocationIds
            .Select(locationId =>
                new DepartmentLocation(DepartmentLocationId.Create(), departmentId, LocationId.Of(locationId)));

        var beginTransactionResult = await _transactionManager.BeginTransactionAsync(cancellationToken);

        if (beginTransactionResult.IsFailure)
            return beginTransactionResult.Error.ToErrors();

        using var transactionScope = beginTransactionResult.Value;

        department.SetLocations(locationsIds);

        await _departmentRepository.DeleteLocationsById(departmentId, cancellationToken);

        var commitedResult = await _transactionManager.SaveChangesAsyncWithResult(cancellationToken);

        if (commitedResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error.ToErrors();

            return commitedResult.Error.ToErrors();
        }

        var transactionCommitedResult = transactionScope.Commit();

        if (transactionCommitedResult.IsFailure)
            return transactionCommitedResult.Error.ToErrors();

        await _cache.RemoveByPrefixAsync(CachingKeys.DEPARTMENTS_KEY, cancellationToken);

        _logger.LogInformation("New locations have been set for the department by id {DepartmentId}", departmentId.Value);

        return departmentId.Value;
    }
}