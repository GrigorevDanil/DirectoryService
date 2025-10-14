using CSharpFunctionalExtensions;

namespace Shared.Abstractions;

/// <summary>
/// Обработчик команд с ответом
/// </summary>
/// <typeparam name="TCommand">Команда.</typeparam>
/// <typeparam name="TResponse">Ответ.</typeparam>
public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand
{
    /// <summary>
    /// Обработка команды
    /// </summary>
    /// <param name="command">Команда.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Вернет указанные данные в TResponse или список ошибок <see cref="Errors"/>.</returns>
    Task<Result<TResponse, Errors>> Handle(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
/// Обработчик команд без ответа
/// </summary>
/// <typeparam name="TCommand">Команда.</typeparam>
public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    /// <summary>
    /// Обработка команды
    /// </summary>
    /// <param name="command">Команда.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список ошибок <see cref="Errors"/>.</returns>
    Task<UnitResult<Errors>> Handle(TCommand command, CancellationToken cancellationToken = default);
}