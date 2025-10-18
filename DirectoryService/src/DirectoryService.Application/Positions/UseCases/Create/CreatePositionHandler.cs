using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments;
using DirectoryService.Domain.DepartmentPositions;
using DirectoryService.Domain.DepartmentPositions.ValueObjects;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Shared.Validation;

namespace DirectoryService.Application.Positions.UseCases.Create;

public class CreatePositionHandler : ICommandHandler<CreatePositionCommand, Guid>
{
    private readonly IPositionsRepository _positionsRepository;

    private readonly IDepartmentRepository _departmentRepository;

    private readonly IValidator<CreatePositionCommand> _validator;

    private readonly ILogger<CreatePositionHandler> _logger;

    private readonly ITransactionManager _transactionManager;

    public CreatePositionHandler(
        IPositionsRepository positionsRepository,
        IValidator<CreatePositionCommand> validator,
        ILogger<CreatePositionHandler> logger,
        IDepartmentRepository departmentRepository,
        ITransactionManager transactionManager)
    {
        _positionsRepository = positionsRepository;
        _validator = validator;
        _logger = logger;
        _departmentRepository = departmentRepository;
        _transactionManager = transactionManager;
    }

    public async Task<Result<Guid, Errors>> Handle(
        CreatePositionCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrors();

        var positionId = PositionId.Create();

        var name = PositionName.Of(command.Request.Name).Value;

        var description = Description.Of(command.Request.Description).Value;

        var checkExistingIdsResult = await _departmentRepository.CheckExistingAndActiveIds(command.Request.DepartmentIds, cancellationToken);

        if (checkExistingIdsResult.IsFailure)
            return checkExistingIdsResult.Error;

        var departmentIds = command.Request.DepartmentIds
            .Select(departmentId =>
                new DepartmentPosition(DepartmentPositionId.Create(), DepartmentId.Of(departmentId), positionId));

        var position = new Position(positionId, name, description, departmentIds);

        await _positionsRepository.AddPositionAsync(position, cancellationToken);

        var commitedResult = await _transactionManager.SaveChangesAsyncWithResult(cancellationToken);

        if (commitedResult.IsFailure) 
            return commitedResult.Error.ToErrors();

        _logger.LogInformation("Position by id {positionId} has been added.", positionId.Value);

        return positionId.Value;
    }
}