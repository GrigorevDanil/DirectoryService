using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.Locations.ValueObjects;

/// <summary>
/// Название локации
/// </summary>
public record LocationName
{
    public const int MAX_LENGHT = 120;

    private LocationName(string value) => Value = value;

    public string Value { get; private set; }

    /// <summary>
    /// Создает новый объект "Название локации"
    /// </summary>
    /// <param name="value">Входящее значение.</param>
    /// <returns>Новый объект или ошибка.</returns>
    public static Result<LocationName> Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGHT || value.Length < 3)
            return Result.Failure<LocationName>("Value is empty or does not match the allowed length");

        return new LocationName(value);
    }
};