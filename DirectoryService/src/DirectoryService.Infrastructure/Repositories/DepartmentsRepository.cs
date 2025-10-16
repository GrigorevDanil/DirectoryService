using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Departments.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DirectoryService.Infrastructure.Repositories;

public class DepartmentsRepository : IDepartmentRepository
{
    private readonly AppDbContext _dbContext;

    public DepartmentsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, Error>> AddDepartmentAsync(Department department, CancellationToken cancellationToken = default)
    {
        await _dbContext.Departments.AddAsync(department, cancellationToken);

        var saveChangesResult = await _dbContext.SaveChangesAsyncWithResult(cancellationToken);

        if (saveChangesResult.IsFailure)
            return saveChangesResult.Error;

        return department.Id.Value;
    }

    public async Task<Result<Department, Error>> GetDepartmentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var departmentId = DepartmentId.Of(id);

        var department = await _dbContext.Departments.FirstOrDefaultAsync(x => x.Id == departmentId, cancellationToken);

        if (department is null)
            return GeneralErrors.NotFound(id);

        return department;
    }

    public async Task<UnitResult<Errors>> CheckExistingAndActiveIds(Guid[] ids, CancellationToken cancellationToken = default)
    {
        var departmentIds = DepartmentId.Of(ids);

        var existingIds = await _dbContext.Departments
            .Where(d => departmentIds.Contains(d.Id) && d.IsActive)
            .Select(d => d.Id.Value)
            .ToListAsync(cancellationToken);

        var missingIds = ids.Except(existingIds).ToList();

        var errors = missingIds.Select(missingId => GeneralErrors.NotFound(missingId)).ToList();

        if (errors.Count != 0)
            return new Errors(errors);

        return Result.Success<Errors>();
    }
}