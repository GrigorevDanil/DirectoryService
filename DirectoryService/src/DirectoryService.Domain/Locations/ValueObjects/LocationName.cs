using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Domain.Locations.ValueObjects;

/// <summary>
/// Название локации
/// </summary>
public record LocationName
{
    public const int MIN_LENGHT = 3;
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
        if (string.IsNullOrWhiteSpace(value))
            return GeneralErrors.ValueIsRequired("location.name");

        if (value.Length is > MAX_LENGHT or < MIN_LENGHT)
            return GeneralErrors.ValueIsEmptyOrInvalidLength("location.name");

        return new LocationName(value);
    }
};