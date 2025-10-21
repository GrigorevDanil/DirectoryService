using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Departments;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Departments.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Shared;
using Path = DirectoryService.Domain.Departments.ValueObjects.Path;

namespace DirectoryService.Infrastructure.Repositories;

public class DepartmentsRepository : IDepartmentRepository
{
    private readonly AppDbContext _dbContext;

    public DepartmentsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddDepartmentAsync(Department department, CancellationToken cancellationToken = default)
    {
        await _dbContext.Departments.AddAsync(department, cancellationToken);

        return department.Id.Value;
    }

    public async Task<Result<Department, Error>> GetActiveDepartmentByIdAsync(DepartmentId id, CancellationToken cancellationToken = default)
    {
        var department = await _dbContext.Departments.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true, cancellationToken);

        if (department is null)
            return GeneralErrors.NotFound(id.Value);

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

    public async Task DeleteLocationsById(DepartmentId id, CancellationToken cancellationToken = default)
    {
        await _dbContext.DepartmentLocations
            .Where(x => x.DepartmentId == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<Result<Department, Error>> GetActiveDepartmentByIdAsyncWithLock(
        DepartmentId id,
        CancellationToken cancellationToken = default)
    {
        var department = await _dbContext.Departments
            .FromSql($"SELECT d.* FROM departments d WHERE d.id = {id.Value} AND d.is_active = TRUE FOR UPDATE")
            .FirstOrDefaultAsync(cancellationToken);

        if (department is null)
            return GeneralErrors.NotFound(id.Value);

        return department;
    }

    public async Task<UnitResult<Error>> LockDescending(Path path, CancellationToken cancellationToken = default)
    {
        const string sql = """
                           SELECT d.* FROM departments d
                           WHERE d.path <@ @parentPath::ltree
                           ORDER BY d.depth
                           FOR UPDATE
                           """;

        var dbConnection = _dbContext.Database.GetDbConnection();

        try
        {
            await dbConnection.QueryAsync(
                sql,
                new
                {
                    parentPath = path.Value,
                });
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }

        return Result.Success<Error>();
    }

    public async Task<UnitResult<Error>> MoveDepartment(
        Path departmentPath,
        Path parentPath,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
                           UPDATE departments d
                           SET path = @parentPath::ltree || subpath(path, nlevel(@departmentPath::ltree) - 1)
                           WHERE path <@ @departmentPath::ltree
                           """;

        var dbConnection = _dbContext.Database.GetDbConnection();

        try
        {
            await dbConnection.ExecuteAsync(
                sql,
                new
                {
                    departmentPath = departmentPath.Value,
                    parentPath = parentPath.Value,
                });
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }

        return Result.Success<Error>();
    }

    public async Task<UnitResult<Error>> MoveDepartment(
        Path departmentPath,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
                           UPDATE departments d
                           SET path = subpath(path, nlevel(@departmentPath::ltree) - 1)
                           WHERE path <@ @departmentPath::ltree
                           """;

        var dbConnection = _dbContext.Database.GetDbConnection();

        try
        {
            await dbConnection.ExecuteAsync(
                sql,
                new
                {
                    departmentPath = departmentPath.Value,
                });
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }

        return Result.Success<Error>();
    }

    public async Task<UnitResult<Error>> CheckParentIsChild(
        Path departmentPath,
        Path parentPath,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
                           SELECT d.id FROM departments d
                           WHERE d.path <@ @departmentPath::ltree AND d.path = @parentPath::ltree
                           ORDER BY d.depth
                           """;

        var dbConnection = _dbContext.Database.GetDbConnection();

        try
        {
            var resultQuery = await dbConnection.QueryAsync(
                sql,
                new
                {
                    departmentPath = departmentPath.Value,
                    parentPath = parentPath.Value,
                });

            if (resultQuery.Any())
                return GeneralErrors.ValueIsInvalid("ParentId indicates the child department", "department.parentId");
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }

        return Result.Success<Error>();
    }
}