namespace DirectoryService.Domain.Departments.ValueObjects;

/// <summary>
/// Уникальный идентификатор подразделения
/// </summary>
public record DepartmentId
{
    private DepartmentId(Guid value) => Value = value;

    public Guid Value { get; private set; }

    /// <summary>
    /// Создание нового идентификатора для подразделения
    /// </summary>
    /// <returns>Новый идентификатор подразделения.</returns>
    public static DepartmentId Create() => new(Guid.NewGuid());

    /// <summary>
    /// Создание идентификатора подразделения из входящего идентификатора
    /// </summary>
    /// <param name="departmentId">Входящий идентификатор.</param>
    /// <returns>Идентификатор подразделения.</returns>
    public static DepartmentId Of(Guid departmentId) => new(departmentId);
};