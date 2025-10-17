using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Constants;
using Shared.Database;

namespace DirectoryService.Infrastructure.Database;

/// <summary>
/// Менеджер для управления транзакциями
/// </summary>
public class TransactionManager : ITransactionManager
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<TransactionManager> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public TransactionManager(
        AppDbContext dbContext,
        ILogger<TransactionManager> logger,
        ILoggerFactory loggerFactory)
    {
        _dbContext = dbContext;
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    public async Task<Result<ITransactionScope, Error>> BeginTransaction(CancellationToken cancellationToken = default)
    {
        try
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            var logger = _loggerFactory.CreateLogger<TransactionScope>();

            var transactionScope = new TransactionScope(transaction.GetDbTransaction(), logger);

            return transactionScope;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Transaction could not be started");

            return GeneralErrors.Failure("Transaction could not be started");
        }
    }

    public async Task<UnitResult<Error>> SaveChangesAsyncWithResult(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success<Error>();
        }
        catch (DbUpdateException dbUpdateEx)
        {
            if (dbUpdateEx.InnerException?.Data[InnerExceptionDataConstants.SQL_STATE]!.ToString() ==
                SqlStates.UNIQUE_CONSTRAINT_VIOLATION)
            {
                _logger.LogError(dbUpdateEx, "Duplicate record");
                return GeneralErrors.Conflict();
            }

            _logger.LogError(dbUpdateEx, "Database update error");
            return GeneralErrors.Failure(dbUpdateEx.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database update error");
            return GeneralErrors.Failure(ex.Message);
        }
    }
}