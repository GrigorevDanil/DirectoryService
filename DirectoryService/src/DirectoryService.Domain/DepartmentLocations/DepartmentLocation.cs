using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Locations.ValueObjects;

namespace DirectoryService.Domain.DepartmentLocations;

/// <summary>
/// Связанная сущность между подразделениями и локациями
/// </summary>
public class DepartmentLocation
{
    public DepartmentLocation(
        DepartmentId departmentId,
        LocationId locationId)
    {
        DepartmentId = departmentId;
        LocationId = locationId;
    }

    public DepartmentId DepartmentId { get; private set; }

    public LocationId LocationId { get; private set; }
}