using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Domain.Departments.ValueObjects;

/// <summary>
/// Название подразделения
/// </summary>
public record DepartmentName
{
    public const int MIN_LENGHT = 3;
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
        if (string.IsNullOrWhiteSpace(value))
            return GeneralErrors.ValueIsRequired("department.name");

        if (value.Length is > MAX_LENGHT or < MIN_LENGHT)
            return GeneralErrors.ValueIsInvalidLength("department.name");

        return new DepartmentName(value);
    }
};