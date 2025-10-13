﻿using CSharpFunctionalExtensions;
using DirectoryService.Domain.DepartmentLocations;
using DirectoryService.Domain.DepartmentPositions;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Locations.ValueObjects;
using DirectoryService.Domain.Positions.ValueObjects;
using DirectoryService.Domain.Shared;
using Shared;
using Path = DirectoryService.Domain.Departments.ValueObjects.Path;

namespace DirectoryService.Domain.Departments;

/// <summary>
/// Сущность подразделения
/// </summary>
public sealed class Department : BaseEntity<DepartmentId>, ISoftDeletable
{
    private readonly List<Department> _children = [];

    private readonly List<DepartmentLocation> _locations = [];

    private readonly List<DepartmentPosition> _positions = [];

    private Department(
        DepartmentId id,
        DepartmentName name,
        Identifier identifier,
        Path path,
        IEnumerable<DepartmentLocation> locations)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        Path = path;
        _locations = locations.ToList();
    }

    /// <summary>
    /// Конструктор для работы EF
    /// </summary>
    private Department() { }

    public DepartmentName Name { get; private set; } = null!;

    public Identifier Identifier { get; private set; } = null!;

    public DepartmentId? ParentId { get; private set; }

    public Path Path { get; private set; } = null!;

    public bool IsActive { get; private set; } = true;

    public IReadOnlyList<DepartmentLocation> Locations => _locations;

    public IReadOnlyList<DepartmentPosition> Positions => _positions;

    public IReadOnlyList<Department> Children => _children;


    /// <summary>
    /// Создать родительское подразделение
    /// </summary>
    /// <param name="name">Название подразделения.</param>
    /// <param name="identifier">Короткое название подразделения.</param>
    /// <param name="locations">Локации.</param>
    /// <param name="departmentId">Идентификатор подразделения.</param>
    /// <returns>Новый объект <see cref="Department"/> или ошибка <see cref="Error"/>.</returns>
    public static Result<Department, Error> CreateParent(
        DepartmentName name,
        Identifier identifier,
        IEnumerable<DepartmentLocation> locations,
        DepartmentId? departmentId = null)
    {
        var locationsList = locations.ToList();

        if (locationsList.Count == 0)
            return GeneralErrors.ValueIsInvalid("Department locations must contain at least one location", "department.location");

        var path = Path.CreateParent(identifier);

        return new Department(departmentId ?? DepartmentId.Create(), name, identifier, path, locationsList);
    }

    /// <summary>
    /// Создать дочернее подразделение
    /// </summary>
    /// <param name="name">Название подразделения.</param>
    /// <param name="identifier">Короткое название подразделения.</param>
    /// <param name="parent">Родительское подразделение.</param>
    /// <param name="locations">Локации.</param>
    /// <param name="departmentId">Идентификатор подразделения.</param>
    /// <returns>Новый объект <see cref="Department"/> или ошибка <see cref="Error"/>.</returns>
    public static Result<Department, Error> CreateChild(
        DepartmentName name,
        Identifier identifier,
        Department parent,
        IEnumerable<DepartmentLocation> locations,
        DepartmentId? departmentId = null)
    {
        var locationsList = locations.ToList();

        if (locationsList.Count == 0)
            return GeneralErrors.ValueIsInvalid("Department locations must contain at least one location", "department.location");

        var path = parent.Path.CreateChild(identifier);

        return new Department(departmentId ?? DepartmentId.Create(), name, identifier, path, locationsList);
    }

    /// <summary>
    /// Переименовать название подразделения
    /// </summary>
    /// <param name="name">Новое название подразделения.</param>
    /// <returns>Результат выполнения переименования.</returns>
    public UnitResult<Error> Rename(string name)
    {
        var nameResult = DepartmentName.Of(name);

        if (nameResult.IsFailure)
            return nameResult.Error;

        Name = nameResult.Value;

        return Result.Success<Error>();
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
    /// <returns>Объект <see cref="DepartmentLocation"/> или ошибку <see cref="Error"/>.</returns>
    public Result<DepartmentLocation, Error> GetLocationById(LocationId locationId)
    {
        var location = _locations.FirstOrDefault(x => x.LocationId == locationId);

        if (location is null)
            return GeneralErrors.NotFound(locationId.Value);

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
    /// <returns>Объект <see cref="DepartmentPosition"/> или ошибку <see cref="Error"/>.</returns>
    public Result<DepartmentPosition, Error> GetPositionById(PositionId positionId)
    {
        var position = _positions.FirstOrDefault(x => x.PositionId == positionId);

        if (position is null)
            return GeneralErrors.NotFound(positionId.Value);

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