using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Domain.Departments.ValueObjects;

/// <summary>
/// Краткое название подразделения для формирования пути
/// </summary>
public partial record Identifier
{
    // Проверяет что бы значение являлось латиницей, что бы в название мог быть дефис, а так же что название не содержит пробелы
    [GeneratedRegex("^[a-zA-Z]+(?:-[a-zA-Z]+)*$")]
    private static partial Regex IsLatin();

    public const int MIN_LENGHT = 3;
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
        if (string.IsNullOrWhiteSpace(value))
            return GeneralErrors.ValueIsRequired("department.identifier");

        if (value.Length is > MAX_LENGHT or < MIN_LENGHT)
            return GeneralErrors.ValueIsInvalidLength("department.identifier");

        if (!IsLatin().IsMatch(value))
            return GeneralErrors.ValueIsInvalid("Value is not Latin", "department.identifier");

        return new Identifier(value);
    }

};