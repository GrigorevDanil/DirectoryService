using DirectoryService.Contracts.Departments.Dtos;

namespace DirectoryService.Contracts.Positions.Dtos;

public record PositionDetailDto
{
    public Guid Id { get; init; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public List<DepartmentShortDto> Departments { get; init; } = [];

    public int CountDepartments => Departments.Count;

    public bool IsActive { get; private set; } = true;

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }

    public DateTime? DeletedAt { get; init; }
}