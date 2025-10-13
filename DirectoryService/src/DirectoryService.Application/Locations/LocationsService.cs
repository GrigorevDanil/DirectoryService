using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Dtos;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Locations.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared;

namespace DirectoryService.Application.Locations;


public class LocationsService : ILocationsService
{
    private readonly ILocationsRepository _locationsRepository;

    private readonly ILogger<LocationsService> _logger;

    public LocationsService(ILocationsRepository locationsRepository, ILogger<LocationsService> logger)
    {
        _locationsRepository = locationsRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Errors>> AddAsync(LocationDto locationDto, CancellationToken cancellationToken)
    {
        var errors = new List<Error>();

        var nameResult = LocationName.Of(locationDto.Name);

        if (nameResult.IsFailure)
            errors.Add(nameResult.Error);

        var timezoneResult = Timezone.Of(locationDto.Timezone);

        if (timezoneResult.IsFailure)
            errors.Add(timezoneResult.Error);

        var addressResult = Address.Of(locationDto.Address);

        if (addressResult.IsFailure)
            errors.AddRange(addressResult.Error);

        if (errors.Count != 0)
            return new Errors(errors);

        var name = nameResult.Value;

        var timezone = timezoneResult.Value;

        var address = addressResult.Value;

        var location = new Location(
            LocationId.Create(),
            name,
            timezone,
            address);

        var addLocationResult = await _locationsRepository.AddLocationAsync(location, cancellationToken);

        if (addLocationResult.IsFailure)
            return addLocationResult.Error.ToErrors();

        var locationId = addLocationResult.Value;

        _logger.LogInformation("Location by id {locationId} has been added.", locationId);

        return locationId;
    }
}