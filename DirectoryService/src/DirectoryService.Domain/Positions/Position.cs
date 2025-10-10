using CSharpFunctionalExtensions;
using DirectoryService.Domain.Positions.ValueObjects;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Positions;

/// <summary>
/// Сущность позиции(должности сотрудника)
/// </summary>
public class Position : BaseEntity<PositionId>, ISoftDeletable
{
    public Position(
        PositionId id,
        PositionName name,
        Description description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Конструктор для работы EF
    /// </summary>
    private Position() { }

    public PositionName Name { get; private set; } = null!;

    public Description Description { get; private set; } = null!;

    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Переименовать название позиции(должности сотрудника)
    /// </summary>
    /// <param name="name">Новое название позиции(должности сотрудника).</param>
    /// <returns>Результат выполнения переименования.</returns>
    public Result Rename(string name)
    {
        var nameResult = PositionName.Of(name);

        if (nameResult.IsFailure)
            return Result.Failure(nameResult.Error);

        Name = nameResult.Value;

        return Result.Success();
    }

    /// <summary>
    /// Изменить описание позиции(должности сотрудника)
    /// </summary>
    /// <param name="description">Новое описание позиции(должности сотрудника).</param>
    /// <returns>Результат выполнения изменения описания.</returns>
    public Result ChangeDescription(string description)
    {
        var descriptionResult = Description.Of(description);

        if (descriptionResult.IsFailure)
            return Result.Failure(descriptionResult.Error);

        Description = descriptionResult.Value;

        return Result.Success();
    }

    /// <summary>
    /// Помечает сущность как удаленную
    /// </summary>
    public void MarkAsDelete()
    {
        IsActive = false;
    }
}