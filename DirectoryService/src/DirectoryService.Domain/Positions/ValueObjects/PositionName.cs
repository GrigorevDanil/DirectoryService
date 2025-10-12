using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Domain.Positions.ValueObjects;

/// <summary>
/// Название позиции(должности сотрудника)
/// </summary>
public record PositionName
{
    public const int MAX_LENGHT = 100;

    private PositionName(string value) => Value = value;

    public string Value { get; private set; }

    /// <summary>
    /// Создает новый объект <see cref="PositionName"/>
    /// </summary>
    /// <param name="value">Входящее значение.</param>
    /// <returns>Новый объект <see cref="PositionName"/> или ошибка <see cref="Error"/>.</returns>
    public static Result<PositionName, Error> Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGHT || value.Length < 3)
            return GeneralErrors.ValueIsEmptyOrInvalidLength("name");

        return new PositionName(value);
    }
};