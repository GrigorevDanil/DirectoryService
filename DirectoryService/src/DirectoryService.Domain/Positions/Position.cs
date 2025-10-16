using CSharpFunctionalExtensions;
using DirectoryService.Domain.DepartmentPositions;
using DirectoryService.Domain.Positions.ValueObjects;
using Shared;

namespace DirectoryService.Domain.Positions;

/// <summary>
/// Сущность позиции(должности сотрудника)
/// </summary>
public sealed class Position : BaseEntity<PositionId>, ISoftDeletable
{
    private readonly List<DepartmentPosition> _departments = [];

    public Position(
        PositionId id,
        PositionName name,
        Description description,
        IEnumerable<DepartmentPosition> departments)
    {
        Id = id;
        Name = name;
        Description = description;
        _departments = departments.ToList();
    }

    /// <summary>
    /// Конструктор для работы EF
    /// </summary>
    private Position() { }

    public PositionName Name { get; private set; } = null!;

    public Description Description { get; private set; } = null!;

    public IReadOnlyList<DepartmentPosition> Departments => _departments;

    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Переименовать название позиции(должности сотрудника)
    /// </summary>
    /// <param name="name">Новое название позиции(должности сотрудника).</param>
    /// <returns>Результат выполнения переименования.</returns>
    public UnitResult<Error> Rename(string name)
    {
        var nameResult = PositionName.Of(name);

        if (nameResult.IsFailure)
            return nameResult.Error;

        Name = nameResult.Value;

        return Result.Success<Error>();
    }

    /// <summary>
    /// Изменить описание позиции(должности сотрудника)
    /// </summary>
    /// <param name="description">Новое описание позиции(должности сотрудника).</param>
    /// <returns>Результат выполнения изменения описания.</returns>
    public UnitResult<Error> ChangeDescription(string description)
    {
        var descriptionResult = Description.Of(description);

        if (descriptionResult.IsFailure)
            return descriptionResult.Error;

        Description = descriptionResult.Value;

        return Result.Success<Error>();
    }

    /// <summary>
    /// Помечает сущность как удаленную
    /// </summary>
    public void MarkAsDelete()
    {
        IsActive = false;
    }
}