using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Dtos;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Locations.ValueObjects;

namespace DirectoryService.Application.Locations;


public class LocationsService : ILocationsService
{
    private readonly ILocationRepository _locationRepository;

    public LocationsService(ILocationRepository locationRepository) => _locationRepository = locationRepository;

    public async Task<Result<Guid>> AddAsync(LocationDto locationDto, CancellationToken cancellationToken)
    {
        var nameResult = LocationName.Of(locationDto.Name);

        if (nameResult.IsFailure)
            return Result.Failure<Guid>(nameResult.Error);

        var name = nameResult.Value;


        var timezoneResult = Timezone.Of(locationDto.Timezone);

        if (timezoneResult.IsFailure)
            return Result.Failure<Guid>(timezoneResult.Error);

        var timezone = timezoneResult.Value;

        var addressResult = Address.Of(locationDto.Address);

        if (addressResult.IsFailure)
            return Result.Failure<Guid>(addressResult.Error);

        var address = addressResult.Value;

        var location = new Location(
            LocationId.Create(),
            name,
            timezone,
            address);

        var addLocationResult = await _locationRepository.AddLocationAsync(location, cancellationToken);

        if (addLocationResult.IsFailure)
            return Result.Failure<Guid>(addLocationResult.Error);

        var locationId = addLocationResult.Value;

        return Result.Success(locationId);
    }
}