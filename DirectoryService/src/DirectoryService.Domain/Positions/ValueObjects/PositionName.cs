using CSharpFunctionalExtensions;

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
    /// Создает новый объект "Название позиции(должности сотрудника)"
    /// </summary>
    /// <param name="value">Входящее значение.</param>
    /// <returns>Новый объект или ошибка.</returns>
    public static Result<PositionName> Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGHT || value.Length < 3)
            return Result.Failure<PositionName>("Value is empty or does not match the allowed length");

        return new PositionName(value);
    }
};