using DirectoryService.Contracts.DepartmentLocations.Dtos;

namespace DirectoryService.Contracts.Departments.Dtos;

public record DepartmentDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Identifier { get; init; } = string.Empty;

    public Guid? ParentId { get; init; }

    public string Path { get; init; } = string.Empty;

    public int Depth { get; init; }

    public bool IsActive { get; init; }

    public List<DepartmentDto> Children { get; init; } = [];

    public bool HasMoreChildren { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }

    public DateTime? DeletedAt { get; init; }
}