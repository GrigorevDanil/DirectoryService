using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Domain.Departments.ValueObjects;

/// <summary>
/// Название подразделения
/// </summary>
public record DepartmentName
{
    public const int MAX_LENGHT = 150;

    private DepartmentName(string value) => Value = value;

    public string Value { get; private set; }

    /// <summary>
    /// Создает новый объект <see cref="DepartmentName"/>
    /// </summary>
    /// <param name="value">Входящее значение.</param>
    /// <returns>Новый объект <see cref="DepartmentName"/> или ошибка <see cref="Error"/>.</returns>
    public static Result<DepartmentName, Error> Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGHT || value.Length < 3)
            return GeneralErrors.ValueIsEmptyOrInvalidLength("name");

        return new DepartmentName(value);
    }
};