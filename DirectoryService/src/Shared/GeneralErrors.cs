namespace Shared;

/// <summary>
/// Общие ошибки
/// </summary>
public static class GeneralErrors
{
    /// <summary>
    /// Ошибка в случае если значение пустое или превышает длину символов
    /// </summary>
    /// <param name="invalidField">Поле в котором произошла ошибка.</param>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <returns>Экземпляр <see cref="Error"/>.</returns>
    public static Error ValueIsEmptyOrInvalidLength(
        string? invalidField = null,
        string message = "Value is empty or does not match the allowed length") => Error.Validation(message, invalidField);

    /// <summary>
    /// Ошибка в случае если значение некорректное
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="invalidField">Поле в котором произошла ошибка.</param>
    /// <returns>Экземпляр <see cref="Error"/>.</returns>
    public static Error ValueIsInvalid(
        string message, string? invalidField = null) => Error.Validation(message, invalidField);

    /// <summary>
    /// Ошибка в случае если запись не найдена
    /// </summary>
    /// <param name="id">Идентификатор не найденной записи.</param>
    /// <returns>Экземпляр <see cref="Error"/>.</returns>
    public static Error NotFound(Guid? id = null)
    {
        string byId = id != null ? $"by {id} " : string.Empty;
        return Error.NotFound($"Record {byId}not found");
    }

    /// <summary>
    /// Ошибка сервера
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <returns>Экземпляр <see cref="Error"/>.</returns>
    public static Error Failure(string message)
    {
        return Error.Failure(message);
    }
}