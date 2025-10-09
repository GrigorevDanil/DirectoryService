using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.Departments.ValueObjects;

/// <summary>
/// Краткое название подразделения для формирования пути
/// </summary>
public record Identifier
{
    public const int MAX_LENGHT = 150;

    private Identifier(string value) => Value = value;

    public string Value { get; private set; }

    /// <summary>
    /// Создает новый объект "Краткое название подразделения"
    /// </summary>
    /// <param name="value">Входящее значение.</param>
    /// <returns>Новый объект или ошибка.</returns>
    public static Result<Identifier> Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGHT || value.Length < 3)
            return Result.Failure<Identifier>("Value is empty or does not match the allowed length");

        // Проверяет что бы значение являлось латиницей, что бы в название мог быть дефис, а так же что название не содержит пробелы
        if (Regex.IsMatch(value, "^[a-zA-Z]+(?:-[a-zA-Z]+)*$"))
            return Result.Failure<Identifier>("Value is not Latin");

        return new Identifier(value);
    }
};