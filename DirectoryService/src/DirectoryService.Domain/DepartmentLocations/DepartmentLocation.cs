using DirectoryService.Domain.DepartmentLocations.ValueObjects;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Locations.ValueObjects;

namespace DirectoryService.Domain.DepartmentLocations;

/// <summary>
/// Связанная сущность между подразделениями и локациями
/// </summary>
public class DepartmentLocation
{
    public DepartmentLocation(
        DepartmentLocationId departmentLocationId,
        DepartmentId departmentId,
        LocationId locationId)
    {
        Id = departmentLocationId;
        DepartmentId = departmentId;
        LocationId = locationId;
    }

    /// <summary>
    /// Конструктор для работы EF
    /// </summary>
    private DepartmentLocation() { }

    public DepartmentLocationId Id { get; private set; } = null!;

    public DepartmentId DepartmentId { get; private set; } = null!;

    public LocationId LocationId { get; private set; } = null!;
}