namespace DirectoryService.Domain.Positions.ValueObjects;

/// <summary>
/// Уникальный идентификатор позиции(должности сотрудника)
/// </summary>
public record PositionId
{
    private PositionId(Guid value) => Value = value;

    public Guid Value { get; private set; }

    /// <summary>
    /// Создание нового идентификатора для позиции(должности сотрудника)
    /// </summary>
    /// <returns>Новый идентификатор позиции(должности сотрудника).</returns>
    public static PositionId Create() => new(Guid.NewGuid());

    /// <summary>
    /// Создание идентификатора позиции(должности сотрудника) из входящего идентификатора
    /// </summary>
    /// <param name="positionId">Входящий идентификатор.</param>
    /// <returns>Идентификатор позиции(должности сотрудника).</returns>
    public static PositionId Of(Guid positionId) => new(positionId);
};