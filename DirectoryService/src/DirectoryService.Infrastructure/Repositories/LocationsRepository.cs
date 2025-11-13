using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Locations;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Locations.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DirectoryService.Infrastructure.Repositories;

public class LocationsRepository : ILocationRepository
{
    private readonly AppDbContext _dbContext;

    public LocationsRepository(AppDbContext dbContext) => _dbContext = dbContext;


    public async Task<Guid> AddLocationAsync(Location location, CancellationToken cancellationToken = default)
    {
        await _dbContext.Locations.AddAsync(location, cancellationToken);

        return location.Id.Value;
    }

    public async Task<UnitResult<Error>> DeleteUnusedLocationsByDepartmentIdAsync(
        DepartmentId id,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
                           UPDATE locations
                                     SET is_active = false,
                                         deleted_at = NOW()
                                     WHERE id IN (
                                         SELECT dl.location_id
                                         FROM department_locations dl
                                         WHERE dl.department_id = @DepartmentId
                                         AND NOT EXISTS (
                                             SELECT 1 
                                             FROM department_locations dl2 
                                             JOIN departments d ON dl2.department_id = d.id
                                             WHERE dl2.location_id = dl.location_id 
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

    public async Task<Result<Location, Error>> GetLocationByIdAsync(LocationId id, CancellationToken cancellationToken = default)
    {
        var location = await _dbContext.Locations.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (location is null)
            return GeneralErrors.NotFound(id.Value);

        return location;
    }

    public async Task<UnitResult<Errors>> CheckExistingAndActiveIds(Guid[] ids, CancellationToken cancellationToken = default)
    {
        var locationIds = LocationId.Of(ids);

        var existingIds = await _dbContext.Locations
            .Where(l => locationIds.Contains(l.Id) && l.IsActive)
            .Select(l => l.Id.Value)
            .ToListAsync(cancellationToken);

        var missingIds = ids.Except(existingIds).ToList();

        var errors = missingIds.Select(missingId => GeneralErrors.NotFound(missingId)).ToList();

        if (errors.Count != 0)
            return new Errors(errors);

        return Result.Success<Errors>();
    }
}