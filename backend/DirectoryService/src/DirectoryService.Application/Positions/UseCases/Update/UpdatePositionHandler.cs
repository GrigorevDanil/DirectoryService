using CSharpFunctionalExtensions;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using SharedService.Core.Database;
using SharedService.Core.Handlers;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Positions.UseCases.Update;

public class UpdatePositionHandler : ICommandHandler<UpdatePositionCommand, Guid>
{
    private readonly IPositionsRepository _positionsRepository;

    private readonly ILogger<UpdatePositionHandler> _logger;

    private readonly IValidator<UpdatePositionCommand> _validator;

    private readonly ITransactionManager _transactionManager;

    public UpdatePositionHandler(
        IPositionsRepository positionsRepository,
        ILogger<UpdatePositionHandler> logger,
        IValidator<UpdatePositionCommand> validator,
        ITransactionManager transactionManager)
    {
        _positionsRepository = positionsRepository;
        _logger = logger;
        _validator = validator;
        _transactionManager = transactionManager;
    }

    public async Task<Result<Guid, Errors>> Handle(
        UpdatePositionCommand command,
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

        position.Rename(command.Request.Name);

        position.ChangeDescription(command.Request.Description);

        UnitResult<Error> commitedResult = await _transactionManager.SaveChangesAsyncWithResult(cancellationToken);

        if (commitedResult.IsFailure)
            return commitedResult.Error.ToErrors();

        _logger.LogInformation("Position by id {PositionId} has been updated", command.Id);

        return command.Id;
    }
}