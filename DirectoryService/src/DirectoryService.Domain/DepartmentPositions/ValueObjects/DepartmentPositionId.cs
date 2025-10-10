namespace DirectoryService.Domain.DepartmentPositions.ValueObjects;

/// <summary>
/// Уникальный идентификатор связанной сущности между подразделением и позиции(должности сотрудника)
/// </summary>
public record DepartmentPositionId
{
    private DepartmentPositionId(Guid value) => Value = value;

    public Guid Value { get; private set; }

    /// <summary>
    /// Создание нового идентификатора для связанной сущности между подразделением и позиции(должности сотрудника)
    /// </summary>
    /// <returns>Новый идентификатор связанной сущности между подразделением и позиции(должности сотрудника).</returns>
    public static DepartmentPositionId Create() => new(Guid.NewGuid());

    /// <summary>
    /// Создание идентификатора связанной сущности между подразделением и позиции(должности сотрудника) из входящего идентификатора
    /// </summary>
    /// <param name="departmentPositionId">Входящий идентификатор.</param>
    /// <returns>Идентификатор связанной сущности между подразделением и позиции(должности сотрудника).</returns>
    public static DepartmentPositionId Of(Guid departmentPositionId) => new(departmentPositionId);
};