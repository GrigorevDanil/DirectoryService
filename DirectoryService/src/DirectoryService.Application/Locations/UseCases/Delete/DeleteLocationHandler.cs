using CSharpFunctionalExtensions;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Locations.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedService.Core.Database;
using SharedService.Core.Handlers;
using SharedService.Core.Validation;
using SharedService.SharedKernel;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace DirectoryService.Application.Locations.UseCases.Delete;

public class DeleteLocationHandler : ICommandHandler<DeleteLocationCommand, Guid>
{
    private readonly ILocationRepository _locationsRepository;

    private readonly ILogger<DeleteLocationHandler> _logger;

    private readonly IValidator<DeleteLocationCommand> _validator;

    private readonly ITransactionManager _transactionManager;

    public DeleteLocationHandler(
        ILocationRepository locationsRepository,
        ILogger<DeleteLocationHandler> logger,
        IValidator<DeleteLocationCommand> validator,
        ITransactionManager transactionManager)
    {
        _locationsRepository = locationsRepository;
        _logger = logger;
        _validator = validator;
        _transactionManager = transactionManager;
    }

    public async Task<Result<Guid, Errors>> Handle(
        DeleteLocationCommand command,
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

        location.MarkAsDelete();

        UnitResult<Error> commitedResult = await _transactionManager.SaveChangesAsyncWithResult(cancellationToken);

        if (commitedResult.IsFailure)
            return commitedResult.Error.ToErrors();

        _logger.LogInformation("Location by id {LocationId} has been deleted", command.LocationId);

        return command.LocationId;
    }
}