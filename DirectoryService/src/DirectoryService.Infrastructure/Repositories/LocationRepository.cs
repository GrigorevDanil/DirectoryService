using CSharpFunctionalExtensions;
using DirectoryService.Application.Locations;
using DirectoryService.Domain.Locations;

namespace DirectoryService.Infrastructure.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly AppDbContext _dbContext;

    public LocationRepository(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task<Result<Guid>> AddLocationAsync(Location location, CancellationToken cancellationToken)
    {
        await _dbContext.Locations.AddAsync(location, cancellationToken);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>(ex.Message);
        }

        return Result.Success(location.Id.Value);
    }
}