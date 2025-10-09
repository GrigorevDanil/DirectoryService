using CSharpFunctionalExtensions;
using DirectoryService.Domain.DepartmentLocations;
using DirectoryService.Domain.DepartmentPositions;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Locations.ValueObjects;
using DirectoryService.Domain.Positions.ValueObjects;
using DirectoryService.Domain.Shared;
using Path = DirectoryService.Domain.Departments.ValueObjects.Path;

namespace DirectoryService.Domain.Departments;

/// <summary>
/// Сущность подразделения
/// </summary>
public class Department : BaseEntity<DepartmentId>, ISoftDeletable
{
    private readonly List<Department> _children = [];

    private readonly List<DepartmentLocation> _locations;

    private readonly List<DepartmentPosition> _positions;

    public Department(
        DepartmentId id,
        DepartmentName name,
        Identifier identifier,
        DepartmentId parentId,
        IEnumerable<DepartmentLocation> locations,
        IEnumerable<DepartmentPosition> positions)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        _locations = locations.ToList();
        _positions = positions.ToList();
    }

    public DepartmentName Name { get; private set; }

    public Identifier Identifier { get; private set; }

    public DepartmentId? ParentId { get; private set; }

    public Path Path { get; private set; } = null!;

    public bool IsActive { get; private set; } = true;

    public IReadOnlyList<DepartmentLocation> Locations => _locations;

    public IReadOnlyList<DepartmentPosition> Positions => _positions;

    public IReadOnlyList<Department> Children => _children;

    /// <summary>
    /// Устанавливает путь для подразделения исходя из короткого название подразделения
    /// </summary>
    /// <param name="identifier">Короткое название подразделения.</param>
    /// <param name="path">Путь родителя.</param>
    /// <returns>Результат установки пути.</returns>
    public Result SetPath(Identifier identifier, Path? path = null)
    {
        // Подраздел является родительским
        if (path == null)
        {
            var resultPath = Path.Of(identifier.Value);

            if (resultPath.IsFailure)
                return Result.Failure(resultPath.Error);

            Path = resultPath.Value;

            return Result.Success();
        }

        // Подраздел является дочерним
        var resultDescendingPath = Path.Descending(identifier.Value);

        if (resultDescendingPath.IsFailure)
            return Result.Failure(resultDescendingPath.Error);

        Path = resultDescendingPath.Value;

        return Result.Success();
    }

    /// <summary>
    /// Переименовать название подразделения
    /// </summary>
    /// <param name="name">Новое название подразделения.</param>
    /// <returns>Результат выполнения переименования.</returns>
    public Result Rename(string name)
    {
        var nameResult = DepartmentName.Of(name);

        if (nameResult.IsFailure)
            return Result.Failure(nameResult.Error);

        Name = nameResult.Value;

        return Result.Success();
    }

    /// <summary>
    /// Изменить короткое название подразделения
    /// </summary>
    /// <param name="identifier">Новое короткое название подразделения.</param>
    /// <returns>Результат выполнения переименования.</returns>
    public Result ChangeIdentifier(string identifier)
    {
        var identifierResult = Identifier.Of(identifier);

        if (identifierResult.IsFailure)
            return Result.Failure(identifierResult.Error);

        var newIdentifier = identifierResult.Value;

        var changeSegmentResult = Path.ChangeSegment(Identifier.Value, newIdentifier.Value);

        if (changeSegmentResult.IsFailure)
            return Result.Failure(changeSegmentResult.Error);

        foreach (var child in _children)
        {
            var childChangeSegmentResult = child.Path.ChangeSegment(Identifier.Value, newIdentifier.Value);

            if (childChangeSegmentResult.IsFailure)
                return Result.Failure(childChangeSegmentResult.Error);

            child.Path = childChangeSegmentResult.Value;
        }

        Identifier = newIdentifier;

        Path = changeSegmentResult.Value;

        return Result.Success();
    }

    /// <summary>
    /// Добавляет дочерний подраздел к родительскому подразделу
    /// </summary>
    /// <param name="department">Дочерний подраздел.</param>
    public void AddDepartment(Department department)
    {
        _children.Add(department);
    }

    /// <summary>
    /// Удалить дочерний подраздел из родительского подраздела
    /// </summary>
    /// <param name="department">Дочерний подраздел.</param>
    public void RemoveDepartment(Department department)
    {
        _children.Remove(department);
    }

    /// <summary>
    /// Добавляет локацию к подразделению
    /// </summary>
    /// <param name="location">Локация.</param>
    public void AddLocation(DepartmentLocation location)
    {
        _locations.Add(location);
    }

    /// <summary>
    /// Удалить локацию из подразделения
    /// </summary>
    /// <param name="location">Локация.</param>
    public void RemoveLocation(DepartmentLocation location)
    {
        _locations.Remove(location);
    }

    /// <summary>
    /// Ищет локацию в подразделении по идентификатору
    /// </summary>
    /// <param name="locationId">Идентификатор локации.</param>
    /// <returns>Связанная сущность локации и подразделения или ошибку.</returns>
    public Result<DepartmentLocation> GetLocationById(LocationId locationId)
    {
        var location = _locations.FirstOrDefault(x => x.LocationId == locationId);

        if (location is null)
            return Result.Failure<DepartmentLocation>("Location not found by id " + locationId.Value);

        return location;
    }

    /// <summary>
    /// Добавляет позицию(должность сотрудника) к подразделению
    /// </summary>
    /// <param name="position">Позиция.</param>
    public void AddPosition(DepartmentPosition position)
    {
        _positions.Add(position);
    }

    /// <summary>
    /// Удалить позицию(должность сотрудника) из подразделения
    /// </summary>
    /// <param name="position">Позиция.</param>
    public void RemovePosition(DepartmentPosition position)
    {
        _positions.Remove(position);
    }

    /// <summary>
    /// Ищет позицию(должность сотрудника) в подразделении по идентификатору
    /// </summary>
    /// <param name="positionId">Идентификатор позиции(должность сотрудника).</param>
    /// <returns>Связанная сущность позиции(должность сотрудника) и подразделения или ошибку.</returns>
    public Result<DepartmentPosition> GetPositionById(PositionId positionId)
    {
        var position = _positions.FirstOrDefault(x => x.PositionId == positionId);

        if (position is null)
            return Result.Failure<DepartmentPosition>("Position not found by id " + positionId.Value);

        return position;
    }

    /// <summary>
    /// Помечает сущность как удаленную
    /// </summary>
    public void MarkAsDelete()
    {
        IsActive = false;
    }
}