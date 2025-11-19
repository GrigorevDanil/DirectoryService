using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Departments;
using DirectoryService.Contracts.Departments.Dtos;
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

    public async Task<Result<DepartmentDtoOnlyWithPath[], Error>> GetOutdatedDepartmentsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
                           SELECT id, path, depth 
                           FROM departments
                           WHERE is_active = false AND deleted_at < NOW() - INTERVAL '1 month'
                           """;

        var dbConnection = _dbContext.Database.GetDbConnection();

        try
        {
            var departmentDtos = await dbConnection.QueryAsync<DepartmentDtoOnlyWithPath>(sql);

            return departmentDtos.ToArray();
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }
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

    public async Task<UnitResult<Error>> DeleteDepartmentLocationsByIdsAsync(
        DepartmentId[] departmentIds,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.DepartmentLocations
                .Where(x => departmentIds.Contains(x.DepartmentId) )
                .ExecuteDeleteAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }

        return UnitResult.Success<Error>();
    }

    public async Task<UnitResult<Error>> DeleteDepartmentPositionsByIdsAsync(
        DepartmentId[] departmentIds,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.DepartmentPositions
                .Where(x => departmentIds.Contains(x.DepartmentId))
                .ExecuteDeleteAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }

        return UnitResult.Success<Error>();
    }

    public async Task<UnitResult<Error>> UpdatePathsBeforeDeleteDepartments(
        Path[] departmentPaths,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
                        UPDATE departments 
                        SET path = (SELECT text2ltree(split_part(@departmentPath::text, '.', nlevel(@departmentPath::ltree)))),
                            depth = 1,
                            parent_id = NULL
                        WHERE path = @departmentPath::ltree;
                        
                        WITH updated_paths AS (
                            SELECT 
                                id,
                                subpath(path, 0, nlevel(@departmentPath::ltree) - 1) || subpath(path, nlevel(@departmentPath::ltree)) as new_path
                            FROM departments 
                            WHERE path <@ @departmentPath::ltree AND path != @departmentPath::ltree
                        )
                        UPDATE departments 
                        SET 
                            path = up.new_path,
                            depth = nlevel(up.new_path),
                            parent_id = (SELECT id FROM departments WHERE path = subpath(up.new_path, 0, nlevel(up.new_path) - 1))
                        FROM updated_paths up
                        WHERE departments.id = up.id;
                        """;

        var dbConnection = _dbContext.Database.GetDbConnection();

        try
        {
            foreach (var path in departmentPaths)
            {
                await dbConnection.ExecuteAsync(sql, new { departmentPath = path.Value, });
            }
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }

        return UnitResult.Success<Error>();
    }

    public async Task<UnitResult<Error>> DeleteDepartmentsByIdsAsync(DepartmentId[] departmentIds, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Departments
                .Where(x => departmentIds.Contains(x.Id))
                .ExecuteDeleteAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }

        return UnitResult.Success<Error>();
    }

}