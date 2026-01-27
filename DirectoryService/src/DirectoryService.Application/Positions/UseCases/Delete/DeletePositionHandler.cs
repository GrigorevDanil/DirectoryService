using CSharpFunctionalExtensions;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using SharedService.Core.Database;
using SharedService.Core.Handlers;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Positions.UseCases.Delete;

public class DeletePositionHandler : ICommandHandler<DeletePositionCommand, Guid>
{
    private readonly IPositionsRepository _positionsRepository;

    private readonly ILogger<DeletePositionHandler> _logger;

    private readonly IValidator<DeletePositionCommand> _validator;

    private readonly ITransactionManager _transactionManager;

    public DeletePositionHandler(
        IPositionsRepository positionsRepository,
        ILogger<DeletePositionHandler> logger,
        IValidator<DeletePositionCommand> validator,
        ITransactionManager transactionManager)
    {
        _positionsRepository = positionsRepository;
        _logger = logger;
        _validator = validator;
        _transactionManager = transactionManager;
    }

    public async Task<Result<Guid, Errors>> Handle(
        DeletePositionCommand command,
        CancellationToken cancellationToken = new CancellationToken())
    {
        ValidationResult validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrors();

        var positionId = PositionId.Of(command.Id);

        Result<Position, Error> getPositionResult = await _positionsRepository.GetBy(x => x.Id == positionId, cancellationToken);

        if (getPositionResult.IsFailure)
            return getPositionResult.Error.ToErrors();

        Position position = getPositionResult.Value;

        position.MarkAsDelete();

        UnitResult<Error> commitedResult = await _transactionManager.SaveChangesAsyncWithResult(cancellationToken);

        if (commitedResult.IsFailure)
            return commitedResult.Error.ToErrors();

        _logger.LogInformation("Position by id {LocationId} has been deleted", command.Id);

        return command.Id;
    }
}