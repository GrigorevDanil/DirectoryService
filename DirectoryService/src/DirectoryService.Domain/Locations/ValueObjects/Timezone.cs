using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Domain.Locations.ValueObjects;

/// <summary>
/// Часовой пояс представленный в виде IANA
/// </summary>
public record Timezone
{
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
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGHT || value.Length < 3)
            return GeneralErrors.ValueIsEmptyOrInvalidLength("timezone");

        if (!TimeZoneInfo.TryFindSystemTimeZoneById(value, out _))
            return GeneralErrors.ValueIsInvalid("Value is not IANA", "timezone");

        return new Timezone(value);
    }
};