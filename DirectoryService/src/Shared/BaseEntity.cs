using Shared.Abstractions;

namespace Shared;

/// <summary>
/// Базовая сущность
/// </summary>
/// <typeparam name="TEntityId">Идентификатор сущности.</typeparam>
public class BaseEntity<TEntityId>
{
    /// <summary>
    /// Идентификатор сущности.
    /// </summary>
    public TEntityId Id { get; init; } = default!;

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    /// <summary>
    /// Дата последнего изменения
    /// </summary>
    public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;
}