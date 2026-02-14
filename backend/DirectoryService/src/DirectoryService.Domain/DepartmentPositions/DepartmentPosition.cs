using DirectoryService.Domain.DepartmentPositions.ValueObjects;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Positions.ValueObjects;

namespace DirectoryService.Domain.DepartmentPositions;

/// <summary>
/// Связанная сущность между подразделениями и позициями(должностями сотрудников).
/// </summary>
public sealed class DepartmentPosition
{
    public DepartmentPosition(
        DepartmentPositionId departmentPositionId,
        DepartmentId departmentId,
        PositionId positionId)
    {
        Id = departmentPositionId;
        DepartmentId = departmentId;
        PositionId = positionId;
    }

    /// <summary>
    /// Конструктор для работы EF.
    /// </summary>
    private DepartmentPosition() { }

    public DepartmentPositionId Id { get; private set; } = null!;

    public DepartmentId DepartmentId { get; private set; } = null!;

    public PositionId PositionId { get; private set; } = null!;
}