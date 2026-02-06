using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments;
using DirectoryService.Domain.DepartmentPositions;
using DirectoryService.Domain.DepartmentPositions.ValueObjects;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedService.Core.Database;
using SharedService.Core.Handlers;
using SharedService.Core.Validation;
using SharedService.SharedKernel;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace DirectoryService.Application.Positions.UseCases.AddDepartments;

public class AddDepartmentsToPositionHandler : ICommandHandler<AddDepartmentsToPositionCommand, Guid>
{
    private readonly IPositionsRepository _positionsRepository;

    private readonly IDepartmentRepository _departmentRepository;

    private readonly ILogger<AddDepartmentsToPositionHandler> _logger;

    private readonly IValidator<AddDepartmentsToPositionCommand> _validator;

    private readonly ITransactionManager _transactionManager;

    public AddDepartmentsToPositionHandler(
        IPositionsRepository positionsRepository,
        ILogger<AddDepartmentsToPositionHandler> logger,
        IValidator<AddDepartmentsToPositionCommand> validator,
        ITransactionManager transactionManager,
        IDepartmentRepository departmentRepository)
    {
        _positionsRepository = positionsRepository;
        _logger = logger;
        _validator = validator;
        _transactionManager = transactionManager;
        _departmentRepository = departmentRepository;
    }

    public async Task<Result<Guid, Errors>> Handle(
        AddDepartmentsToPositionCommand command,
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

        UnitResult<Errors> checkExistingIdsResult = await _departmentRepository.CheckExistingAndActiveIds(command.Request.DepartmentIds, cancellationToken);

        if (checkExistingIdsResult.IsFailure)
            return checkExistingIdsResult.Error;

        DepartmentPosition[] departmentPositions = command.Request.DepartmentIds
            .Select(departmentId =>
                new DepartmentPosition(DepartmentPositionId.Create(), DepartmentId.Of(departmentId), positionId)).ToArray();

        UnitResult<Errors> addedResult = position.AddDepartments(departmentPositions);

        if (addedResult.IsFailure)
            return addedResult.Error;

        UnitResult<Error> commitedResult = await _transactionManager.SaveChangesAsyncWithResult(cancellationToken);

        if (commitedResult.IsFailure)
            return commitedResult.Error.ToErrors();

        _logger.LogInformation("Departments with ids {DepartmentId} was added to position with id {PositionId}", string.Join(", ", command.Request.DepartmentIds), command.Id);

        return command.Id;
    }
}