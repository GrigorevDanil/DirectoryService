using System.Text.Json.Serialization;

namespace Shared;

/// <summary>
/// Обертка ответа
/// </summary>
public record Envelope
{
    /// <summary>
    /// Результат запроса
    /// </summary>
    public object? Result { get; }

    /// <summary>
    /// Список ошибок
    /// </summary>
    public Errors? ErrorList { get; }

    /// <summary>
    /// Дата вызова
    /// </summary>
    public DateTime TimeGenerated { get; }

    /// <summary>
    /// Содержит ли ответ ошибку
    /// </summary>
    public bool IsError => ErrorList != null || (ErrorList != null && ErrorList.Any());

    [JsonConstructor]
    private Envelope(object? result, Errors? errorList)
    {
        Result = result;
        ErrorList = errorList;
        TimeGenerated = DateTime.UtcNow;
    }

    /// <summary>
    /// Создание объекта <see cref="Envelope"/> в случае успеха
    /// </summary>
    /// <param name="result">Результат запроса.</param>
    /// <returns>Новый объект <see cref="Envelope"/>.</returns>
    public static Envelope Ok(object? result = null) => new(result, null);

    /// <summary>
    /// Создание объекта <see cref="Envelope"/> в случае ошибки
    /// </summary>
    /// <param name="errorList">Список ошибок.</param>
    /// <returns>Новый объект <see cref="Envelope"/>.</returns>
    public static Envelope Error(Errors? errorList) => new(null, errorList);
}

/// <summary>
/// Обертка ответа
/// </summary>
public record Envelope<T>
{
    /// <summary>
    /// Результат запроса
    /// </summary>
    public T? Result { get; }

    /// <summary>
    /// Список ошибок
    /// </summary>
    public Errors? ErrorList { get; }

    /// <summary>
    /// Дата вызова
    /// </summary>
    public DateTime TimeGenerated { get; }

    /// <summary>
    /// Содержит ли ответ ошибку
    /// </summary>
    public bool IsError => ErrorList != null || (ErrorList != null && ErrorList.Any());

    [JsonConstructor]
    private Envelope(T? result, Errors? errorList)
    {
        Result = result;
        ErrorList = errorList;
        TimeGenerated = DateTime.UtcNow;
    }

    /// <summary>
    /// Создание объекта <see cref="Envelope"/> в случае успеха
    /// </summary>
    /// <param name="result">Результат запроса.</param>
    /// <returns>Новый объект <see cref="Envelope"/>.</returns>
    public static Envelope<T> Ok(T? result = default) => new(result, null);

    /// <summary>
    /// Создание объекта <see cref="Envelope"/> в случае ошибки
    /// </summary>
    /// <param name="errorList">Список ошибок.</param>
    /// <returns>Новый объект <see cref="Envelope"/>.</returns>
    public static Envelope<T> Error(Errors? errorList) => new(default, errorList);
}