using CSharpFunctionalExtensions;
using Shared;

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
    /// Создает новый объект <see cref="Description"/>
    /// </summary>
    /// <param name="value">Входящее значение.</param>
    /// <returns>Новый объект <see cref="Description"/> или ошибка <see cref="Error"/>.</returns>
    public static Result<Description, Error> Of(string? value)
    {
        if (value != null)
        {
            if (value.Length > MAX_LENGHT)
                return GeneralErrors.ValueIsInvalidLength("position.description");
        }

        return new Description(value);
    }
};