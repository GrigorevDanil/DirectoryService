using CSharpFunctionalExtensions;
using SharedService.SharedKernel;

namespace DirectoryService.Domain.Positions.ValueObjects;

/// <summary>
/// Название позиции(должности сотрудника).
/// </summary>
public record PositionName
{
    public const int MIN_LENGHT = 3;
    public const int MAX_LENGHT = 100;

    private PositionName(string value) => Value = value;

    public string Value { get; private set; }

    /// <summary>
    /// Создает новый объект <see cref="PositionName"/>.
    /// </summary>
    /// <param name="value">Входящее значение.</param>
    /// <returns>Новый объект <see cref="PositionName"/> или ошибка <see cref="Error"/>.</returns>
    public static Result<PositionName, Error> Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return GeneralErrors.ValueIsRequired("position.name");

        if (value.Length is > MAX_LENGHT or < MIN_LENGHT)
            return GeneralErrors.ValueIsInvalidLength("position.name");

        return new PositionName(value);
    }
}