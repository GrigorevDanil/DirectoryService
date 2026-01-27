using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using SharedService.Core.Database;
using SharedService.Core.Handlers;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Positions.UseCases.RemoveDepartment;

public class RemoveDepartmentsFromPositionHandler : ICommandHandler<RemoveDepartmentsFromPositionCommand, Guid>
{
    private readonly IPositionsRepository _positionsRepository;

    private readonly IDepartmentRepository _deparmentRepository;

    private readonly ILogger<RemoveDepartmentsFromPositionHandler> _logger;

    private readonly IValidator<RemoveDepartmentsFromPositionCommand> _validator;

    private readonly ITransactionManager _transactionManager;

    public RemoveDepartmentsFromPositionHandler(
        IPositionsRepository positionsRepository,
        IDepartmentRepository deparmentRepository,
        ILogger<RemoveDepartmentsFromPositionHandler> logger,
        IValidator<RemoveDepartmentsFromPositionCommand> validator,
        ITransactionManager transactionManager)
    {
        _positionsRepository = positionsRepository;
        _deparmentRepository = deparmentRepository;
        _logger = logger;
        _validator = validator;
        _transactionManager = transactionManager;
    }

    public async Task<Result<Guid, Errors>> Handle(
        RemoveDepartmentsFromPositionCommand command,
        CancellationToken cancellationToken = new CancellationToken())
    {
        ValidationResult validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrors();

        var positionId = PositionId.Of(command.Id);

        Result<Position, Error> getPositionResult = await _positionsRepository.GetWithDepartmentsBy(x => x.Id == positionId, cancellationToken);

        if (getPositionResult.IsFailure)
            return getPositionResult.Error.ToErrors();

        Position position = getPositionResult.Value;

        DepartmentId[] departmentIds = DepartmentId.Of(command.Request.DepartmentIds);

        UnitResult<Errors> removedResult = position.RemoveDepartments(departmentIds);

        if (removedResult.IsFailure)
            return removedResult.Error;

        UnitResult<Error> commitedResult = await _transactionManager.SaveChangesAsyncWithResult(cancellationToken);

        if (commitedResult.IsFailure)
            return commitedResult.Error.ToErrors();

        _logger.LogInformation("Department with id {DepartmentId} was removed from position with id {PositionId}", string.Join(", ", command.Request.DepartmentIds), command.Id);

        return command.Id;
    }
}