using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Positions;
using DirectoryService.Application.SoftDelete;
using SharedService.Core.Database;
using SharedService.SharedKernel;

namespace DirectoryService.Infrastructure.SoftDelete;

public class DeletedRecordsCleanerService : IDeletedRecordsCleanerService
{
    private readonly IDepartmentRepository _departmentRepository;

    private readonly ILocationRepository _locationRepository;

    private readonly IPositionsRepository _positionsRepository;

    private readonly ITransactionManager _transactionManager;

    public DeletedRecordsCleanerService(
        IDepartmentRepository departmentRepository,
        ILocationRepository locationRepository,
        IPositionsRepository positionsRepository,
        ITransactionManager transactionManager)
    {
        _departmentRepository = departmentRepository;
        _locationRepository = locationRepository;
        _positionsRepository = positionsRepository;
        _transactionManager = transactionManager;
    }

    public async Task<UnitResult<Error>> Process(CancellationToken cancellationToken = default)
    {
        var beginTransactionResult = await _transactionManager.BeginTransactionAsync(cancellationToken);

        if (beginTransactionResult.IsFailure)
            return beginTransactionResult.Error;

        using var transactionScope = beginTransactionResult.Value;

        var deleteDepartmentLocationsResult = await _departmentRepository.DeleteDepartmentLocationsAsync(cancellationToken);

        if (deleteDepartmentLocationsResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error;

            return deleteDepartmentLocationsResult.Error;
        }

        var deleteDepartmentPositionsResult = await _departmentRepository.DeleteDepartmentPositionsAsync(cancellationToken);

        if (deleteDepartmentPositionsResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error;

            return deleteDepartmentPositionsResult.Error;
        }

        var updatePathsResult = await _departmentRepository.UpdatePathsBeforeDeleteDepartments(cancellationToken);

        if (updatePathsResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error;

            return updatePathsResult.Error;
        }

        var deleteDepartmentsResult = await _departmentRepository.DeleteDepartmentsAsync(cancellationToken);

        if (deleteDepartmentsResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error;

            return deleteDepartmentsResult.Error;
        }

        var deleteLocationsResult = await _locationRepository.DeleteOutdatedLocationsAsync(cancellationToken);

        if (deleteLocationsResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error;

            return deleteLocationsResult.Error;
        }

        var deletePositionsResult = await _positionsRepository.DeleteOutdatedPositionsAsync(cancellationToken);

        if (deletePositionsResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error;

            return deletePositionsResult.Error;
        }

        var transactionCommitedResult = transactionScope.Commit();

        if (transactionCommitedResult.IsFailure)
            return transactionCommitedResult.Error;

        return UnitResult.Success<Error>();
    }
}