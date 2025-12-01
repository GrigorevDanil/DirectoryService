using CSharpFunctionalExtensions;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Locations.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedService.Core.Database;
using SharedService.Core.Handlers;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Locations.UseCases.Create;

public class CreateLocationHandler : ICommandHandler<CreateLocationCommand, Guid>
{
    private readonly ILocationRepository _locationsRepository;

    private readonly ILogger<CreateLocationHandler> _logger;

    private readonly IValidator<CreateLocationCommand> _validator;

    private readonly ITransactionManager _transactionManager;

    public CreateLocationHandler(
        ILogger<CreateLocationHandler> logger,
        IValidator<CreateLocationCommand> validator,
        ILocationRepository locationsRepository,
        ITransactionManager transactionManager)
    {
        _logger = logger;
        _validator = validator;
        _locationsRepository = locationsRepository;
        _transactionManager = transactionManager;
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

        var locationId = await _locationsRepository.AddLocationAsync(location, cancellationToken);

        var commitedResult = await _transactionManager.SaveChangesAsyncWithResult(cancellationToken);

        if (commitedResult.IsFailure)
            return commitedResult.Error.ToErrors();

        _logger.LogInformation("Location by id {LocationId} has been added.", locationId);

        return locationId;
    }
}