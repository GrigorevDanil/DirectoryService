using CSharpFunctionalExtensions;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Departments.ValueObjects;
using Shared;

namespace DirectoryService.Application.Departments;

public interface IDepartmentRepository
{
    public Task<Guid> AddDepartmentAsync(Department department, CancellationToken cancellationToken = default);

    public Task<Result<Department, Error>> GetDepartmentByIdAsync(DepartmentId id, CancellationToken cancellationToken = default);

    public Task<UnitResult<Errors>> CheckExistingAndActiveIds(Guid[] ids, CancellationToken cancellationToken = default);

    public Task DeleteLocationsById(DepartmentId id, CancellationToken cancellationToken = default);
}