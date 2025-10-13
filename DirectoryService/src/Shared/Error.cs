using System.Text.Json.Serialization;

namespace Shared;

/// <summary>
/// Ошибка
/// </summary>
public record Error
{
    /// <summary>
    /// Код ошибки
    /// </summary>
    public string Code { get; init; }

    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    /// Тип ошибки
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ErrorType Type { get; init; }

    /// <summary>
    /// Поле в котором произошла ошибка
    /// </summary>
    public string? InvalidField { get; init; }

    [JsonConstructor]
    private Error(
        string code,
        string message,
        ErrorType type,
        string? invalidField = null)
    {
        Code = code;
        Message = message;
        Type = type;
        InvalidField = invalidField;
    }

    /// <summary>
    /// Ошибка валидации
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="invalidField">Поле в котором произошла ошибка.</param>
    /// <param name="code">Код ошибки.</param>
    /// <returns>Экземпляр <see cref="Error"/>.</returns>
    public static Error Validation(string message, string? invalidField = null, string code = "VALUE_IS_INVALID") => new(code, message, ErrorType.VALIDATION, invalidField);

    /// <summary>
    /// Запись не найдена
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="code">Код ошибки.</param>
    /// <returns>Экземпляр <see cref="Error"/>.</returns>
    public static Error NotFound(string message, string code = "RECORD_NOT_FOUND") => new(code, message, ErrorType.NOT_FOUND);

    /// <summary>
    /// Ошибка в логике работы
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="code">Код ошибки.</param>
    /// <returns>Экземпляр <see cref="Error"/>.</returns>
    public static Error Failure(string message, string code = "FAILURE") => new(code, message, ErrorType.FAILURE);

    /// <summary>
    /// Конфликт
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="code">Код ошибки.</param>
    /// <returns>Экземпляр <see cref="Error"/>.</returns>
    public static Error Conflict(string message, string code = "VALUE_IS_CONFLICT") => new(code, message, ErrorType.CONFLICT);

    /// <summary>
    /// Преобразует ошибку в список ошибок <see cref="Errors"/>
    /// </summary>
    /// <returns>Список ошибок <see cref="Errors"/>.</returns>
    public Errors ToErrors() => new([this]);
}

/// <summary>
/// Тип ошибки
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Ошибка валидации
    /// </summary>
    VALIDATION,

    /// <summary>
    /// Ошибка ничего не найдено
    /// </summary>
    NOT_FOUND,

    /// <summary>
    /// Ошибка сервера
    /// </summary>
    FAILURE,

    /// <summary>
    /// Ошибка конфликт
    /// </summary>
    CONFLICT,
}