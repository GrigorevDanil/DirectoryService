using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Positions;
using DirectoryService.Application.SoftDelete;
using DirectoryService.Domain.Departments.ValueObjects;
using Shared;
using Shared.Database;
using Path = DirectoryService.Domain.Departments.ValueObjects.Path;

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

        var getDepartmentsResult = await _departmentRepository.GetOutdatedDepartmentsAsync(cancellationToken);

        if (getDepartmentsResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error;

            return getDepartmentsResult.Error;
        }

        var departmentDtos = getDepartmentsResult.Value;

        var departmentIds = DepartmentId.Of(departmentDtos.Select(x => x.Id).ToArray());

        List<Path> departmentPaths = [];

        foreach (string path in departmentDtos.Select(x => x.Path))
        {
            var pathResult = Path.Of(path);

            if (pathResult.IsFailure)
                return pathResult.Error;

            departmentPaths.Add(pathResult.Value);
        }

        var deleteDepartmentLocationsResult = await _departmentRepository.DeleteDepartmentLocationsByIdsAsync(departmentIds, cancellationToken);

        if (deleteDepartmentLocationsResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error;

            return deleteDepartmentLocationsResult.Error;
        }

        var deleteDepartmentPositionsResult = await _departmentRepository.DeleteDepartmentPositionsByIdsAsync(departmentIds, cancellationToken);

        if (deleteDepartmentPositionsResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error;

            return deleteDepartmentPositionsResult.Error;
        }

        var updatePathsResult = await _departmentRepository.UpdatePathsBeforeDeleteDepartments(departmentPaths.ToArray(), cancellationToken);

        if (updatePathsResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error;

            return updatePathsResult.Error;
        }

        var deleteDepartmentsResult = await _departmentRepository.DeleteDepartmentsByIdsAsync(departmentIds, cancellationToken);

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