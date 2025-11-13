using CSharpFunctionalExtensions;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Positions;
using DirectoryService.Domain.Departments.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Shared.Validation;

namespace DirectoryService.Application.Departments.UseCases.Delete;

public class DeleteDepartmentHandler : ICommandHandler<DeleteDepartmentCommand, Guid>
{
    private readonly IDepartmentRepository _departmentRepository;

    private readonly ILocationRepository _locationRepository;

    private readonly IPositionsRepository _positionsRepository;

    private readonly IValidator<DeleteDepartmentCommand> _validator;

    private readonly ILogger<DeleteDepartmentHandler> _logger;

    private readonly ITransactionManager _transactionManager;

    public DeleteDepartmentHandler(
        IDepartmentRepository departmentRepository,
        IValidator<DeleteDepartmentCommand> validator,
        ILogger<DeleteDepartmentHandler> logger,
        ITransactionManager transactionManager,
        ILocationRepository locationRepository,
        IPositionsRepository positionsRepository)
    {
        _departmentRepository = departmentRepository;
        _validator = validator;
        _logger = logger;
        _transactionManager = transactionManager;
        _locationRepository = locationRepository;
        _positionsRepository = positionsRepository;
    }

    public async Task<Result<Guid, Errors>> Handle(
        DeleteDepartmentCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrors();

        var departmentId = DepartmentId.Of(command.Id);

        var beginTransactionResult = await _transactionManager.BeginTransaction(cancellationToken);

        if (beginTransactionResult.IsFailure)
            return beginTransactionResult.Error.ToErrors();

        using var transactionScope = beginTransactionResult.Value;

        var getDepartmentResult = await _departmentRepository.GetActiveDepartmentByIdAsync(departmentId, cancellationToken);

        if (getDepartmentResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error.ToErrors();

            return getDepartmentResult.Error.ToErrors();
        }

        var department = getDepartmentResult.Value;

        department.MarkAsDelete();

        var updatePathsResult = await _departmentRepository.UpdatePathsAfterDelete(department.Path, cancellationToken);

        if (updatePathsResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error.ToErrors();

            return updatePathsResult.Error.ToErrors();
        }

        var deleteLocationsResult = await _locationRepository.DeleteUnusedLocationsByDepartmentIdAsync(departmentId, cancellationToken);

        if (deleteLocationsResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error.ToErrors();

            return deleteLocationsResult.Error.ToErrors();
        }

        var deletePositionsResult = await _positionsRepository.DeleteUnusedPositionsByDepartmentIdAsync(departmentId, cancellationToken);

        if (deletePositionsResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error.ToErrors();

            return deletePositionsResult.Error.ToErrors();
        }

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

        _logger.LogInformation("Soft delete department by id {departmentId}", departmentId.Value);

        return departmentId.Value;
    }
}