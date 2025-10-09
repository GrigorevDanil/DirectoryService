using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Positions.ValueObjects;

namespace DirectoryService.Domain.DepartmentPositions;

/// <summary>
/// Связанная сущность между подразделениями и позициями(должностями сотрудников)
/// </summary>
public class DepartmentPosition
{
    public DepartmentPosition(
        DepartmentId departmentId,
        PositionId positionId)
    {
        DepartmentId = departmentId;
        PositionId = positionId;
    }

    public DepartmentId DepartmentId { get; private set; }

    public PositionId PositionId { get; private set; }
}