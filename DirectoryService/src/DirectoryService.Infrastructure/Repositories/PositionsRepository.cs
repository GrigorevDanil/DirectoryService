using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Positions;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Positions;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DirectoryService.Infrastructure.Repositories;

public class PositionsRepository : IPositionsRepository
{
    private readonly AppDbContext _dbContext;

    public PositionsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddPositionAsync(Position position, CancellationToken cancellationToken = default)
    {
        await _dbContext.Positions.AddAsync(position, cancellationToken);

        return position.Id.Value;
    }

    public async Task<UnitResult<Error>> DeleteUnusedPositionsByDepartmentIdAsync(
        DepartmentId id,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
                           UPDATE positions
                                     SET is_active = false,
                                         deleted_at = NOW()
                                     WHERE id IN (
                                         SELECT dp.position_id
                                         FROM department_positions dp
                                         WHERE dp.department_id = @DepartmentId
                                         AND NOT EXISTS (
                                             SELECT 1 
                                             FROM department_positions dp2 
                                             JOIN departments d ON dp2.department_id = d.id
                                             WHERE dp2.position_id = dp.position_id 
                                             AND d.is_active = true
                                             AND d.id != @DepartmentId
                                         )
                                     ) AND is_active = true;
                           """;

        var dbConnection = _dbContext.Database.GetDbConnection();

        try
        {
            await dbConnection.ExecuteAsync(
                sql,
                new
                {
                    DepartmentId = id.Value,
                });
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }

        return UnitResult.Success<Error>();
    }
}