using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Domain.Departments.ValueObjects;

/// <summary>
/// Краткое название подразделения для формирования пути
/// </summary>
public partial record Identifier
{
    public const int MAX_LENGHT = 150;

    private Identifier(string value) => Value = value;

    public string Value { get; private set; }

    /// <summary>
    /// Создает новый объект <see cref="Identifier"/>
    /// </summary>
    /// <param name="value">Входящее значение.</param>
    /// <returns>Новый объект <see cref="Identifier"/> или ошибка <see cref="Error"/>.</returns>
    public static Result<Identifier, Error> Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGHT || value.Length < 3)
            return GeneralErrors.ValueIsEmptyOrInvalidLength("identifier");

        if (IsLatin().IsMatch(value))
            return GeneralErrors.ValueIsInvalid("Value is not Latin", "identifier");

        return new Identifier(value);
    }

    // Проверяет что бы значение являлось латиницей, что бы в название мог быть дефис, а так же что название не содержит пробелы
    [GeneratedRegex("^[a-zA-Z]+(?:-[a-zA-Z]+)*$")]
    private static partial Regex IsLatin();
};