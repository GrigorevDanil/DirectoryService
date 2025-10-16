using CSharpFunctionalExtensions;
using DirectoryService.Domain.Locations;
using Shared;

namespace DirectoryService.Application.Locations;

public interface ILocationRepository
{
    public Task<Result<Guid, Error>> AddLocationAsync(Location location, CancellationToken cancellationToken = default);

    public Task<Result<Location, Error>> GetLocationByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<UnitResult<Errors>> CheckExistingAndActiveIds(Guid[] ids, CancellationToken cancellationToken = default);
}