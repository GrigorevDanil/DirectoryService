using CSharpFunctionalExtensions;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Locations.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Abstractions;
using Shared.Validation;

namespace DirectoryService.Application.Locations.UseCases.Create;

public class CreateLocationHandler : ICommandHandler<CreateLocationCommand, Guid>
{
    private readonly ILocationsRepository _locationsRepository;

    private readonly ILogger<CreateLocationHandler> _logger;

    private readonly IValidator<CreateLocationCommand> _validator;

    public CreateLocationHandler(
        ILocationsRepository locationsRepository,
        ILogger<CreateLocationHandler> logger,
        IValidator<CreateLocationCommand> validator)
    {
        _locationsRepository = locationsRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, Errors>> Handle(CreateLocationCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrors();

        var name = LocationName.Of(command.Request.Name).Value;

        var timezone = Timezone.Of(command.Request.Timezone).Value;

        var address = Address.Of(command.Request.Address).Value;

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