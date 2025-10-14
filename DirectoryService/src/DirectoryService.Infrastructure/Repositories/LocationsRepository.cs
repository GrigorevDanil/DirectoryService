using CSharpFunctionalExtensions;
using DirectoryService.Application.Locations;
using DirectoryService.Domain.Locations;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DirectoryService.Infrastructure.Repositories;

public class LocationsRepository : ILocationsRepository
{
    private readonly AppDbContext _dbContext;

    public LocationsRepository(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task<Result<Guid, Error>> AddLocationAsync(Location location, CancellationToken cancellationToken)
    {
        await _dbContext.Locations.AddAsync(location, cancellationToken);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException dbUpdateEx)
        {
            if (dbUpdateEx.InnerException?.Data["SqlState"]!.ToString() == "23505")
                return GeneralErrors.Conflict();

            return GeneralErrors.Failure(dbUpdateEx.Message);
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }

        return Result.Success<Guid, Error>(location.Id.Value);
    }
}