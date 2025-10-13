using CSharpFunctionalExtensions;
using Shared;

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
    /// Создает новый объект <see cref="LocationName"/>
    /// </summary>
    /// <param name="value">Входящее значение.</param>
    /// <returns>Новый объект <see cref="LocationName"/> или ошибка <see cref="Error"/>.</returns>
    public static Result<LocationName, Error> Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGHT || value.Length < 3)
            return GeneralErrors.ValueIsEmptyOrInvalidLength("name");

        return new LocationName(value);
    }
};