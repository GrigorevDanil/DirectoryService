namespace Shared;

/// <summary>
/// Интерфейс для реализации мягкого удаления
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// Является ли сущность удаленной
    /// </summary>
    public bool IsActive { get; }

    /// <summary>
    /// Дата последнего изменения
    /// </summary>
    public DateTime? DeletedAt { get; }
}