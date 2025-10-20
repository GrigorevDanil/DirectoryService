﻿using CSharpFunctionalExtensions;
using DirectoryService.Domain.Departments.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Shared.Validation;
using Path = DirectoryService.Domain.Departments.ValueObjects.Path;

namespace DirectoryService.Application.Departments.UseCases.Move;

public class MoveDepartmentHandler : ICommandHandler<MoveDepartmentCommand, Guid>
{
    private readonly IDepartmentRepository _departmentRepository;

    private readonly IValidator<MoveDepartmentCommand> _validator;

    private readonly ILogger<MoveDepartmentHandler> _logger;

    private readonly ITransactionManager _transactionManager;

    public MoveDepartmentHandler(
        IDepartmentRepository departmentRepository,
        IValidator<MoveDepartmentCommand> validator,
        ILogger<MoveDepartmentHandler> logger,
        ITransactionManager transactionManager)
    {
        _departmentRepository = departmentRepository;
        _validator = validator;
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public async Task<Result<Guid, Errors>> Handle(
        MoveDepartmentCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrors();

        var beginTransactionResult = await _transactionManager.BeginTransaction(cancellationToken);

        if (beginTransactionResult.IsFailure)
            return beginTransactionResult.Error.ToErrors();

        var departmentId = DepartmentId.Of(command.DepartmentId);

        using var transactionScope = beginTransactionResult.Value;

        var getDepartmentResult = await _departmentRepository.GetActiveDepartmentByIdAsyncWithLock(departmentId, cancellationToken);

        if (getDepartmentResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error.ToErrors();

            return getDepartmentResult.Error.ToErrors();
        }

        var department = getDepartmentResult.Value;

        await _departmentRepository.LockDescending(department.Path, cancellationToken);

        // Присоединение подразделения к родителю
        if (command.Request.ParentId != null)
        {
            var parentId = DepartmentId.Of(command.Request.ParentId.GetValueOrDefault());

            var getParentResult = await _departmentRepository.GetActiveDepartmentByIdAsyncWithLock(parentId, cancellationToken);

            if (getParentResult.IsFailure)
                return getParentResult.Error.ToErrors();

            var parent = getParentResult.Value;

            var checkParentIsChildResult = await _departmentRepository.CheckParentIsChild(department.Path, parent.Path, cancellationToken);

            if (checkParentIsChildResult.IsFailure)
                return checkParentIsChildResult.Error.ToErrors();

            var moveDepartmentResult =
                await _departmentRepository.MoveDepartment(department.Path, parent.Path, cancellationToken);

            if (moveDepartmentResult.IsFailure)
                return moveDepartmentResult.Error.ToErrors();
        }

        // Подразделение становится корнем
        else
        {
            var moveDepartmentResult =
                await _departmentRepository.MoveDepartment(department.Path, cancellationToken);

            if (moveDepartmentResult.IsFailure)
                return moveDepartmentResult.Error.ToErrors();
        }

        var commitedResult = await _transactionManager.SaveChangesAsyncWithResult(cancellationToken);

        if (commitedResult.IsFailure)
        {
            var rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error.ToErrors();

            return commitedResult.Error.ToErrors();
        }

        var commitedTransactionResult = transactionScope.Commit();

        if (commitedTransactionResult.IsFailure)
            return commitedTransactionResult.Error.ToErrors();

        if (command.Request.ParentId != null)
        {
            _logger.LogInformation(
                "Department by id {departmentId} was added to parent by id {parentId}",
                command.DepartmentId, command.Request.ParentId);
        }
        else
        {
            _logger.LogInformation(
                "Department by id {departmentId} became root",
                command.DepartmentId);
        }

        return departmentId.Value;
    }
}