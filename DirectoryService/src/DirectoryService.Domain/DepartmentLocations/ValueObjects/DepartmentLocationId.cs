namespace DirectoryService.Domain.DepartmentLocations.ValueObjects;

/// <summary>
/// Уникальный идентификатор связанной сущности между подразделением и локацией
/// </summary>
public record DepartmentLocationId
{
    private DepartmentLocationId(Guid value) => Value = value;

    public Guid Value { get; private set; }

    /// <summary>
    /// Создание нового идентификатора для связанной сущности между подразделением и локацией
    /// </summary>
    /// <returns>Новый идентификатор связанной сущности между подразделением и локацией.</returns>
    public static DepartmentLocationId Create() => new(Guid.NewGuid());

    /// <summary>
    /// Создание идентификатора связанной сущности между подразделением и локацией из входящего идентификатора
    /// </summary>
    /// <param name="departmentLocationId">Входящий идентификатор.</param>
    /// <returns>Идентификатор связанной сущности между подразделением и локацией.</returns>
    public static DepartmentLocationId Of(Guid departmentLocationId) => new(departmentLocationId);
};