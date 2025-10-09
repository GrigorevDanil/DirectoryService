using CSharpFunctionalExtensions;

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
    /// Создает новый объект "Название локации"
    /// </summary>
    /// <param name="value">Входящее значение.</param>
    /// <returns>Новый объект или ошибка.</returns>
    public static Result<Timezone> Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGHT || value.Length < 3)
            return Result.Failure<Timezone>("Value is empty or does not match the allowed length");

        if (!TimeZoneInfo.TryFindSystemTimeZoneById(value, out _))
            return Result.Failure<Timezone>("Value is not IANA");

        return new Timezone(value);
    }
};