using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Abstractions.Locations;
using DirectoryService.Domain.Locations;

namespace DirectoryService.Infrastructure.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly AppDbContext _dbContext;

    public LocationRepository(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task<Guid> AddLocationAsync(Location location)
    {
        await _dbContext.Locations.AddAsync(location);
        await _dbContext.SaveChangesAsync();
        return location.Id.Value;
    }
}