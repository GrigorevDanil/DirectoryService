namespace Shared.Abstractions;

/// <summary>
/// Обработчик запроса с ответом
/// </summary>
/// <typeparam name="TQuery">Запрос.</typeparam>
/// <typeparam name="TResponse">Ответ.</typeparam>
public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery
{
    /// <summary>
    /// Обработка запроса
    /// </summary>
    /// <param name="query">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Вернет ответ TResponse.</returns>
    Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken = default);
}

/// <summary>
/// Обработчик ответа
/// </summary>
/// <typeparam name="TResponse">Ответ.</typeparam>
public interface IQueryHandler<TResponse>
{
    /// <summary>
    /// Обработка запроса
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Вернет ответ TResponse.</returns>
    Task<TResponse> Handle(CancellationToken cancellationToken = default);
}