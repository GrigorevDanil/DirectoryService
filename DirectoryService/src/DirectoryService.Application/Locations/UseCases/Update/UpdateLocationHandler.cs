using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Locations.Dtos;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Locations.ValueObjects;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using SharedService.Core.Database;
using SharedService.Core.Handlers;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Locations.UseCases.Update;

public class UpdateLocationHandler : ICommandHandler<UpdateLocationCommand, Guid>
{
    private readonly ILocationRepository _locationsRepository;

    private readonly ILogger<UpdateLocationHandler> _logger;

    private readonly IValidator<UpdateLocationCommand> _validator;

    private readonly ITransactionManager _transactionManager;

    public UpdateLocationHandler(
        ILocationRepository locationsRepository,
        ILogger<UpdateLocationHandler> logger,
        IValidator<UpdateLocationCommand> validator,
        ITransactionManager transactionManager)
    {
        _locationsRepository = locationsRepository;
        _logger = logger;
        _validator = validator;
        _transactionManager = transactionManager;
    }

    public async Task<Result<Guid, Errors>> Handle(
        UpdateLocationCommand command,
        CancellationToken cancellationToken = new CancellationToken())
    {
        ValidationResult validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrors();

        var locationId = LocationId.Of(command.LocationId);

        Result<Location, Error> getLocationResult = await _locationsRepository.GetLocationByIdAsync(locationId, cancellationToken);

        if (getLocationResult.IsFailure)
            return getLocationResult.Error.ToErrors();

        Location location = getLocationResult.Value;

        location.Rename(command.Request.Name);

        location.ChangeTimezone(command.Request.Timezone);

        AddressDto addressDto = command.Request.Address;

        location.ChangeAddress(
            addressDto.Country,
            addressDto.PostalCode,
            addressDto.Region,
            addressDto.City,
            addressDto.Street,
            addressDto.HouseNumber);

        UnitResult<Error> commitedResult = await _transactionManager.SaveChangesAsyncWithResult(cancellationToken);

        if (commitedResult.IsFailure)
            return commitedResult.Error.ToErrors();

        _logger.LogInformation("Location by id {LocationId} has been updated", command.LocationId);

        return command.LocationId;
    }
}