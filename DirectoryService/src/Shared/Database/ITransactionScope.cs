using CSharpFunctionalExtensions;

namespace Shared.Database;

/// <summary>
/// Интерфейс обертки транзакции
/// </summary>
public interface ITransactionScope : IDisposable
{
    /// <summary>
    /// Сохраняет изменения в базу данных
    /// </summary>
    /// <returns>Ошибка <see cref="Error"/>.</returns>
    public UnitResult<Error> Commit();

    /// <summary>
    /// Откатывает изменения в транзакции
    /// </summary>
    /// <returns>Ошибка <see cref="Error"/>.</returns>
    public UnitResult<Error> Rollback();
}