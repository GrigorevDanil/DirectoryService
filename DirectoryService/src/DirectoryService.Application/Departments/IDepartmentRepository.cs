using CSharpFunctionalExtensions;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Departments.ValueObjects;
using SharedService.SharedKernel;
using Path = DirectoryService.Domain.Departments.ValueObjects.Path;

namespace DirectoryService.Application.Departments;

public interface IDepartmentRepository
{
    Task<Guid> AddDepartmentAsync(Department department, CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> MarkPathsAsDeleted(Path departmentPath, CancellationToken cancellationToken = default);

    Task<Result<Department, Error>> GetActiveDepartmentByIdAsync(DepartmentId id, CancellationToken cancellationToken = default);

    Task<UnitResult<Errors>> CheckExistingAndActiveIds(Guid[] ids, CancellationToken cancellationToken = default);

    Task DeleteLocationsById(DepartmentId id, CancellationToken cancellationToken = default);

    Task<Result<Department, Error>> GetActiveDepartmentByIdAsyncWithLock(DepartmentId id, CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> LockDescending(Path path, CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> MoveChildDepartment(Path departmentPath, Path parentPath, CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> MoveDepartmentToRoot(Path departmentPath, CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> CheckParentIsChild(Path departmentPath, Path parentPath, CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> DeleteDepartmentsAsync(CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> DeleteDepartmentLocationsAsync(CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> DeleteDepartmentPositionsAsync(CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> UpdatePathsBeforeDeleteDepartments(CancellationToken cancellationToken = default);
}