using CSharpFunctionalExtensions;

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
    /// Создает новый объект "Название подразделения"
    /// </summary>
    /// <param name="value">Входящее значение.</param>
    /// <returns>Новый объект или ошибка.</returns>
    public static Result<DepartmentName> Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGHT || value.Length < 3)
            return Result.Failure<DepartmentName>("Value is empty or does not match the allowed length");

        return new DepartmentName(value);
    }
};