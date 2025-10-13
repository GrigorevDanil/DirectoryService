using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Domain.Locations.ValueObjects;

/// <summary>
/// Часовой пояс представленный в виде IANA
/// </summary>
public record Timezone
{
    public const int MIN_LENGHT = 3;
    public const int MAX_LENGHT = 50;
    private Timezone(string value) => Value = value;

    public string Value { get; private set; }

    /// <summary>
    /// Создает новый объект <see cref="Timezone"/>
    /// </summary>
    /// <param name="value">Входящее значение.</param>
    /// <returns>Новый объект <see cref="Timezone"/> или ошибка <see cref="Error"/>.</returns>
    public static Result<Timezone, Error> Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return GeneralErrors.ValueIsRequired("location.timezone");

        if (value.Length is > MAX_LENGHT or < MIN_LENGHT)
            return GeneralErrors.ValueIsInvalidLength("location.timezone");

        if (!TimeZoneInfo.TryFindSystemTimeZoneById(value, out _))
            return GeneralErrors.ValueIsInvalid("Value is not IANA", "location.timezone");

        return new Timezone(value);
    }
};