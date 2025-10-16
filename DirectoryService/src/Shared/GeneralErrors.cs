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
    /// Ошибка в случае если значение пустое или превышает длину символов
    /// </summary>
    /// <param name="invalidField">Поле в котором произошла ошибка.</param>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <returns>Экземпляр <see cref="Error"/>.</returns>
    public static Error ValueIsInvalidLength(
        string? invalidField = null,
        string message = "Value is invalid length") => Error.Validation(message, invalidField);

    /// <summary>
    /// Ошибка в случае если значение не указано
    /// </summary>
    /// <param name="invalidField">Поле в котором произошла ошибка.</param>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <returns>Экземпляр <see cref="Error"/>.</returns>
    public static Error ValueIsRequired(
        string? invalidField = null,
        string message = "Value is required") => Error.Validation(message, invalidField);

    /// <summary>
    /// Ошибка в случае если массив не содержит элементы
    /// </summary>
    /// <param name="invalidField">Поле в котором произошла ошибка.</param>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <returns>Экземпляр <see cref="Error"/>.</returns>
    public static Error ArrayIsRequired(
        string? invalidField = null,
        string message = "Array must have at least 1 element") => Error.Validation(message, invalidField);

    /// <summary>
    /// Ошибка в случае если массив содержит дубликаты
    /// </summary>
    /// <param name="invalidField">Поле в котором произошла ошибка.</param>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <returns>Экземпляр <see cref="Error"/>.</returns>
    public static Error ArrayContainsDuplicates(
        string? invalidField = null,
        string message = "Array contains duplicates") => Error.Validation(message, invalidField);

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
        string byId = id != null ? $"by id {id} " : string.Empty;
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

    /// <summary>
    /// Ошибка конфликта
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <returns>Экземпляр <see cref="Error"/>.</returns>
    public static Error Conflict(string message = "Value is conflicting, and it may be a duplicate")
    {
        return Error.Conflict(message);
    }
}