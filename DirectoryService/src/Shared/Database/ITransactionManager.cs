using CSharpFunctionalExtensions;

namespace Shared.Database;

/// <summary>
/// Интерфейс менеджера для управления транзакциями
/// </summary>
public interface ITransactionManager
{
    /// <summary>
    /// Начать транзакцию
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обертка транзакции <see cref="ITransactionScope"/> или ошибка <see cref="Error"/>.</returns>
    public Task<Result<ITransactionScope, Error>> BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Сохранить изменения в бд с результатом
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения сохранения.</returns>
    public Task<UnitResult<Error>> SaveChangesAsyncWithResult(CancellationToken cancellationToken = default);
}