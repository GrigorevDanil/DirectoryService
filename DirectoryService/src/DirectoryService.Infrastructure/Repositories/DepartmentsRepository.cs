using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Departments;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Departments.ValueObjects;
using Microsoft.EntityFrameworkCore;
using SharedService.SharedKernel;
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

    public async Task<UnitResult<Error>> MarkPathsAsDeleted(Path departmentPath, CancellationToken cancellationToken = default)
    {
        string sql;

        if (departmentPath.Depth == 0)
        {
            sql = """
                  UPDATE departments
                  SET path = ('deleted-' || subpath(path, nlevel(@DepartmentPath::ltree) - 1)::text)::ltree
                  WHERE path <@ @DepartmentPath::ltree;
                  """;
        }
        else
        {
            sql = """
                  UPDATE departments
                  SET path = (subpath(path, 0, nlevel(@DepartmentPath::ltree) - 1)::text ||
                              '.deleted-' || subpath(path, nlevel(@DepartmentPath::ltree) - 1)::text)::ltree
                  WHERE path <@ @DepartmentPath::ltree;
                  """;
        }

        var dbConnection = _dbContext.Database.GetDbConnection();

        try
        {
            await dbConnection.ExecuteAsync(
                sql,
                new
                {
                    DepartmentPath = departmentPath.Value,
                });
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }

        return UnitResult.Success<Error>();
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
            .Where(d => departmentIds.AsEnumerable().Contains(d.Id) && d.IsActive)
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

    public async Task<UnitResult<Error>> MoveChildDepartment(
        Path departmentPath,
        Path parentPath,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
                           UPDATE departments d
                           SET 
                               path = @parentPath::ltree || subpath(path, nlevel(@departmentPath::ltree) - 1),
                               depth = nlevel(@parentPath::ltree || subpath(path, nlevel(@departmentPath::ltree) - 1)) - 1,
                               parent_id = (
                                   SELECT id FROM departments 
                                   WHERE path = subpath(@parentPath::ltree || subpath(d.path, nlevel(@departmentPath::ltree) - 1), 0, -1)
                               )
                           WHERE path <@ @departmentPath::ltree;
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

    public async Task<UnitResult<Error>> MoveDepartmentToRoot(
        Path departmentPath,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
                           UPDATE departments d
                           SET 
                               path = subpath(path, nlevel(@departmentPath::ltree) - 1),
                               depth = nlevel(subpath(path, nlevel(@departmentPath::ltree) - 1)) - 1,
                               parent_id = (
                                   SELECT id FROM departments 
                                   WHERE path = subpath(subpath(d.path, nlevel(@departmentPath::ltree) - 1), 0, -1)
                                   AND nlevel(subpath(d.path, nlevel(@departmentPath::ltree) - 1)) > 1
                               )
                           WHERE path <@ @departmentPath::ltree;
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

    public async Task<UnitResult<Error>> DeleteDepartmentLocationsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.DepartmentLocations
                .Where(x => _dbContext.Departments
                    .Where(dep => !dep.IsActive && dep.DeletedAt < DateTime.UtcNow.AddMonths(-1))
                    .Select(dep => dep.Id)
                    .Contains(x.DepartmentId))
                .ExecuteDeleteAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }

        return UnitResult.Success<Error>();
    }

    public async Task<UnitResult<Error>> DeleteDepartmentPositionsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.DepartmentPositions
                .Where(x => _dbContext.Departments
                    .Where(dep => !dep.IsActive && dep.DeletedAt < DateTime.UtcNow.AddMonths(-1))
                    .Select(dep => dep.Id)
                    .Contains(x.DepartmentId))
                .ExecuteDeleteAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }

        return UnitResult.Success<Error>();
    }

    public async Task<UnitResult<Error>> UpdatePathsBeforeDeleteDepartments(CancellationToken cancellationToken = default)
    {
        const string sql = """
                        WITH outdated_departments AS (
                            SELECT path
                            FROM departments 
                            WHERE is_active = false AND deleted_at < now() - INTERVAL '1 month'
                        )
                        UPDATE departments d
                        SET 
                            path = CASE 
                                WHEN d.path = od.path THEN text2ltree(split_part(ltree2text(d.path), '.', 1))
                                WHEN (SELECT depth FROM departments WHERE path = od.path) = 0 THEN subpath(d.path, 1)
                                ELSE subpath(d.path, 0, 1) || subpath(d.path, 2)
                            END,
                            depth = CASE 
                                WHEN d.path = od.path THEN 0
                                ELSE d.depth - 1
                            END,
                            parent_id = CASE 
                                WHEN d.path = od.path THEN NULL
                                WHEN d.depth - 1 = 0 THEN NULL
                                ELSE (
                                    SELECT id FROM departments dp 
                                    WHERE dp.path = subpath(
                                        CASE 
                                            WHEN (SELECT depth FROM departments WHERE path = od.path) = 0 THEN subpath(d.path, 1)
                                            ELSE subpath(d.path, 0, 1) || subpath(d.path, 2)
                                        END, 
                                        0, 
                                        -1
                                    )
                                )
                            END
                        FROM outdated_departments od
                        WHERE d.path <@ od.path;
                        """;

        var dbConnection = _dbContext.Database.GetDbConnection();

        try
        {
            await dbConnection.ExecuteAsync(sql);
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }

        return UnitResult.Success<Error>();
    }

    public async Task<UnitResult<Error>> DeleteDepartmentsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Departments
                .Where(dep => !dep.IsActive && dep.DeletedAt < DateTime.UtcNow.AddMonths(-1))
                .ExecuteDeleteAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }

        return UnitResult.Success<Error>();
    }

}