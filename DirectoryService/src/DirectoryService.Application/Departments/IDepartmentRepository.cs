using CSharpFunctionalExtensions;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Departments.ValueObjects;
using Shared;
using Path = DirectoryService.Domain.Departments.ValueObjects.Path;

namespace DirectoryService.Application.Departments;

public interface IDepartmentRepository
{
    public Task<Guid> AddDepartmentAsync(Department department, CancellationToken cancellationToken = default);

    public Task<UnitResult<Error>> UpdatePathsAfterDelete(Path departmentPath, CancellationToken cancellationToken = default);

    public Task<Result<Department, Error>> GetActiveDepartmentByIdAsync(DepartmentId id, CancellationToken cancellationToken = default);

    public Task<UnitResult<Errors>> CheckExistingAndActiveIds(Guid[] ids, CancellationToken cancellationToken = default);

    public Task DeleteLocationsById(DepartmentId id, CancellationToken cancellationToken = default);

    public Task<Result<Department, Error>> GetActiveDepartmentByIdAsyncWithLock(DepartmentId id, CancellationToken cancellationToken = default);

    public Task<UnitResult<Error>> LockDescending(Path path, CancellationToken cancellationToken = default);

    public Task<UnitResult<Error>> MoveDepartment(Path departmentPath, Path parentPath, CancellationToken cancellationToken = default);

    public Task<UnitResult<Error>> MoveDepartment(Path departmentPath, CancellationToken cancellationToken = default);

    public Task<UnitResult<Error>> CheckParentIsChild(Path departmentPath, Path parentPath, CancellationToken cancellationToken = default);
}