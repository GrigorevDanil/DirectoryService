namespace DirectoryService.Domain.Shared;

/// <summary>
/// Базовая сущность
/// </summary>
/// <typeparam name="T">Идентификатор сущности.</typeparam>
public class BaseEntity<T>
{
    public T Id { get; init; } = default!;

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    /// <summary>
    /// Дата последнего изменения
    /// </summary>
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
}