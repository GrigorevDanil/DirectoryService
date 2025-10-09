namespace DirectoryService.Domain.Shared;

/// <summary>
/// Интерфейс для реализации мягкого удаления
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// Является ли сущность удаленной
    /// </summary>
    public bool IsActive { get; }
}