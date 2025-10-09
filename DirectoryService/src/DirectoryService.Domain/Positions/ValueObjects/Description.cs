using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.Positions.ValueObjects;

/// <summary>
/// Описание позиции(должности сотрудника)
/// </summary>
public record Description
{
    public const int MAX_LENGHT = 1000;

    private Description(string? value) => Value = value;

    public string? Value { get; private set; }

    /// <summary>
    /// Создает новый объект "Название позиции(должности сотрудника)"
    /// </summary>
    /// <param name="value">Входящее значение.</param>
    /// <returns>Новый объект или ошибка.</returns>
    public static Result<Description> Of(string? value)
    {
        if (value != null)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGHT)
                return Result.Failure<Description>("Value is empty or does not match the allowed length");
        }

        return new Description(value);
    }
};