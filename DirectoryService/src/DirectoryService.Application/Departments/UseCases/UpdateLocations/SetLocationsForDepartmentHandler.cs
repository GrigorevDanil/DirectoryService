using CSharpFunctionalExtensions;
using DirectoryService.Application.Locations;
using DirectoryService.Domain.DepartmentLocations;
using DirectoryService.Domain.DepartmentLocations.ValueObjects;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Locations.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Shared.Validation;

namespace DirectoryService.Application.Departments.UseCases.UpdateLocations;

public class SetLocationsForDepartmentHandler : ICommandHandler<SetLocationsForDepartmentCommand, Guid>
{
    private readonly IDepartmentRepository _departmentRepository;

    private readonly ILocationRepository _locationRepository;

    private readonly IValidator<SetLocationsForDepartmentCommand> _validator;

    private readonly ILogger<SetLocationsForDepartmentHandler> _logger;

    private readonly ITransactionManager _transactionManager;

    public SetLocationsForDepartmentHandler(
        IDepartmentRepository departmentRepository,
        IValidator<SetLocationsForDepartmentCommand> validator,
        ILogger<SetLocationsForDepartmentHandler> logger,
        ITransactionManager transactionManager,
        ILocationRepository locationRepository)
    {
        _departmentRepository = departmentRepository;
        _validator = validator;
        _logger = logger;
        _transactionManager = transactionManager;
        _locationRepository = locationRepository;
    }

    public async Task<Result<Guid, Errors>> Handle(
        SetLocationsForDepartmentCommand forDepartmentCommand,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(forDepartmentCommand, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrors();

        var departmentId = DepartmentId.Of(forDepartmentCommand.DepartmentId);

        var getDepartmentResult = await _departmentRepository.GetDepartmentByIdAsync(departmentId, cancellationToken);

        if (getDepartmentResult.IsFailure)
            return getDepartmentResult.Error.ToErrors();

        var department = getDepartmentResult.Value;

        var checkExistingIdsResult = await _locationRepository.CheckExistingAndActiveIds(forDepartmentCommand.ForDepartmentRequest.LocationIds, cancellationToken);

        if (checkExistingIdsResult.IsFailure)
            return checkExistingIdsResult.Error;

        var locationsIds = forDepartmentCommand.ForDepartmentRequest.LocationIds
            .Select(locationId =>
                new DepartmentLocation(DepartmentLocationId.Create(), departmentId, LocationId.Of(locationId)));

        var beginTransactionResult = await _transactionManager.BeginTransaction(cancellationToken);

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

        _logger.LogInformation("New locations have been set for the department by id {departmentId}", departmentId.Value);

        return departmentId.Value;
    }
}