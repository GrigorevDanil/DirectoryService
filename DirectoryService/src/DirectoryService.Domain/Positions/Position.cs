using CSharpFunctionalExtensions;
using DirectoryService.Domain.DepartmentPositions;
using DirectoryService.Domain.DepartmentPositions.ValueObjects;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Positions.ValueObjects;
using SharedService.SharedKernel;

namespace DirectoryService.Domain.Positions;

/// <summary>
/// Сущность позиции(должности сотрудника).
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
    /// Конструктор для работы EF.
    /// </summary>
    private Position() { }

    public PositionName Name { get; private set; } = null!;

    public Description Description { get; private set; } = null!;

    public IReadOnlyList<DepartmentPosition> Departments => _departments;

    public bool IsActive { get; private set; } = true;

    public DateTime? DeletedAt { get; private set; }

    /// <summary>
    /// Переименовать название позиции(должности сотрудника).
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
    /// Изменить описание позиции(должности сотрудника).
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
    /// Добавить подразделения к позиции.
    /// </summary>
    /// <param name="departmentPositions">Добавляемые подразделения.</param>
    /// <returns>Результат добавления подразделений к позиции.</returns>
    public UnitResult<Errors> AddDepartments(DepartmentPosition[] departmentPositions)
    {
        DepartmentId[] intersectedDepartments = _departments
            .Select(x => x.DepartmentId)
            .Intersect(departmentPositions
                .Select(x => x.DepartmentId)).ToArray();

        if (intersectedDepartments.Any())
        {
            return new Errors(intersectedDepartments
                .Select(departmentId => GeneralErrors.Conflict($"Department with id {departmentId} already exists.")));
        }

        _departments.AddRange(departmentPositions);

        return UnitResult.Success<Errors>();
    }

    /// <summary>
    /// Удалить подразделения из позиции.
    /// </summary>
    /// <param name="departmentIds">Идентификаторы удаляемых отделов.</param>
    /// <returns>Результат удаления подразделений из позиции.</returns>
    public UnitResult<Errors> RemoveDepartments(DepartmentId[] departmentIds)
    {
        DepartmentPosition[] beingDeletedDepartmentPositions = _departments.Where(x => departmentIds.Contains(x.DepartmentId)).ToArray();

        if (beingDeletedDepartmentPositions.Length == 0)
        {
            return new Errors(departmentIds
                .Select(id => GeneralErrors.NotFound(id.Value)));
        }

        _departments.RemoveAll(id => departmentIds.Contains(id.DepartmentId));

        return UnitResult.Success<Errors>();
    }

    /// <summary>
    /// Помечает сущность как удаленную.
    /// </summary>
    /// <param name="deletedAt">Дата удаления.</param>
    public void MarkAsDelete(DateTime? deletedAt = null)
    {
        IsActive = false;
        DeletedAt = deletedAt ?? DateTime.UtcNow;
    }
}