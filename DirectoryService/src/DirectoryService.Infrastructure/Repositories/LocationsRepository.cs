using CSharpFunctionalExtensions;
using DirectoryService.Application.Locations;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Locations.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DirectoryService.Infrastructure.Repositories;

public class LocationsRepository : ILocationRepository
{
    private readonly AppDbContext _dbContext;

    public LocationsRepository(AppDbContext dbContext) => _dbContext = dbContext;


    public async Task<Result<Guid, Error>> AddLocationAsync(Location location, CancellationToken cancellationToken = default)
    {
        await _dbContext.Locations.AddAsync(location, cancellationToken);

        var saveChangesResult = await _dbContext.SaveChangesAsyncWithResult(cancellationToken);

        if (saveChangesResult.IsFailure)
            return saveChangesResult.Error;

        return location.Id.Value;
    }

    public async Task<Result<Location, Error>> GetLocationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var locationId = LocationId.Of(id);

        var location = await _dbContext.Locations.FirstOrDefaultAsync(x => x.Id == locationId, cancellationToken);

        if (location is null)
            return GeneralErrors.NotFound(id);

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