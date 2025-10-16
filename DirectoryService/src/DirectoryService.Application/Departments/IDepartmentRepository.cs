using CSharpFunctionalExtensions;
using DirectoryService.Domain.Departments;
using Shared;

namespace DirectoryService.Application.Departments;

public interface IDepartmentRepository
{
    public Task<Result<Guid, Error>> AddDepartmentAsync(Department department, CancellationToken cancellationToken = default);

    public Task<Result<Department, Error>> GetDepartmentByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<UnitResult<Errors>> CheckExistingAndActiveIds(Guid[] ids, CancellationToken cancellationToken = default);
}