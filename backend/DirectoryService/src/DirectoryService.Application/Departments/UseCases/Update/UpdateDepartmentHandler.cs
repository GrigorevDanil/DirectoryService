using CSharpFunctionalExtensions;
using DirectoryService.Application.Constants;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Departments.ValueObjects;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using SharedService.Core.Caching;
using SharedService.Core.Database;
using SharedService.Core.Handlers;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Departments.UseCases.Update;

public class UpdateDepartmentHandler : ICommandHandler<UpdateDepartmentCommand, Guid>
{
    private readonly IDepartmentRepository _departmentRepository;

    private readonly IValidator<UpdateDepartmentCommand> _validator;

    private readonly ILogger<UpdateDepartmentHandler> _logger;

    private readonly ITransactionManager _transactionManager;

    private readonly ICacheService _cache;

    public UpdateDepartmentHandler(
        IDepartmentRepository departmentRepository,
        IValidator<UpdateDepartmentCommand> validator,
        ILogger<UpdateDepartmentHandler> logger,
        ITransactionManager transactionManager,
        ICacheService cache)
    {
        _departmentRepository = departmentRepository;
        _validator = validator;
        _logger = logger;
        _transactionManager = transactionManager;
        _cache = cache;
    }

    public async Task<Result<Guid, Errors>> Handle(
        UpdateDepartmentCommand command,
        CancellationToken cancellationToken = new CancellationToken())
    {
        ValidationResult validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrors();

        var departmentId = DepartmentId.Of(command.Id);

        Result<ITransactionScope, Error> beginTransactionResult = await _transactionManager.BeginTransactionAsync(cancellationToken);

        if (beginTransactionResult.IsFailure)
            return beginTransactionResult.Error.ToErrors();

        using ITransactionScope transactionScope = beginTransactionResult.Value;

        Result<Department, Error> getDepartmentResult = await _departmentRepository.GetByAsync(x => x.Id == departmentId && x.IsActive == true, cancellationToken);

        if (getDepartmentResult.IsFailure)
        {
            UnitResult<Error> rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error.ToErrors();

            return getDepartmentResult.Error.ToErrors();
        }

        Department department = getDepartmentResult.Value;

        department.Rename(command.Request.Name);

        department.ChangeIdentifier(command.Request.Identifier);

        UnitResult<Error> updatePathsResult = await _departmentRepository.UpdatePathsBeforeChangeIdentifier(departmentId, department.Identifier, cancellationToken);

        if (updatePathsResult.IsFailure)
        {
            UnitResult<Error> rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error.ToErrors();

            return updatePathsResult.Error.ToErrors();
        }

        UnitResult<Error> commitedResult = await _transactionManager.SaveChangesAsyncWithResult(cancellationToken);

        if (commitedResult.IsFailure)
        {
            UnitResult<Error> rollbackResult = transactionScope.Rollback();

            if (rollbackResult.IsFailure)
                return rollbackResult.Error.ToErrors();

            return commitedResult.Error.ToErrors();
        }

        UnitResult<Error> commitedTransactionResult = transactionScope.Commit();

        if (commitedTransactionResult.IsFailure)
            return commitedTransactionResult.Error.ToErrors();

        await _cache.RemoveByPrefixAsync(CachingKeys.DEPARTMENTS_KEY, cancellationToken);

        _logger.LogInformation("Department by id {DepartmentId} updated", departmentId.Value);

        return departmentId.Value;
    }
}