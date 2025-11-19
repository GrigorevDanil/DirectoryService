using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Departments.Dtos;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Departments.ValueObjects;
using Shared;
using Path = DirectoryService.Domain.Departments.ValueObjects.Path;

namespace DirectoryService.Application.Departments;

public interface IDepartmentRepository
{
    public Task<Guid> AddDepartmentAsync(Department department, CancellationToken cancellationToken = default);

    public Task<UnitResult<Error>> MarkPathsAsDeleted(Path departmentPath, CancellationToken cancellationToken = default);

    public Task<Result<Department, Error>> GetActiveDepartmentByIdAsync(DepartmentId id, CancellationToken cancellationToken = default);

    public Task<UnitResult<Errors>> CheckExistingAndActiveIds(Guid[] ids, CancellationToken cancellationToken = default);

    public Task DeleteLocationsById(DepartmentId id, CancellationToken cancellationToken = default);

    public Task<Result<Department, Error>> GetActiveDepartmentByIdAsyncWithLock(DepartmentId id, CancellationToken cancellationToken = default);

    public Task<Result<DepartmentDtoOnlyWithPath[], Error>> GetOutdatedDepartmentsAsync(CancellationToken cancellationToken = default);

    public Task<UnitResult<Error>> LockDescending(Path path, CancellationToken cancellationToken = default);

    public Task<UnitResult<Error>> MoveChildDepartment(Path departmentPath, Path parentPath, CancellationToken cancellationToken = default);

    public Task<UnitResult<Error>> MoveDepartmentToRoot(Path departmentPath, CancellationToken cancellationToken = default);

    public Task<UnitResult<Error>> CheckParentIsChild(Path departmentPath, Path parentPath, CancellationToken cancellationToken = default);

    public Task<UnitResult<Error>> DeleteDepartmentsByIdsAsync(DepartmentId[] departmentIds, CancellationToken cancellationToken = default);

    public Task<UnitResult<Error>> DeleteDepartmentLocationsByIdsAsync(DepartmentId[] departmentIds, CancellationToken cancellationToken = default);

    public Task<UnitResult<Error>> DeleteDepartmentPositionsByIdsAsync(DepartmentId[] departmentIds, CancellationToken cancellationToken = default);

    public Task<UnitResult<Error>> UpdatePathsBeforeDeleteDepartments(Path[] paths, CancellationToken cancellationToken = default);
}