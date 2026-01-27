namespace DirectoryService.Contracts.Positions.Dtos;

public record PositionDto
{
    public Guid Id { get; init; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public string CountDepartments { get; private set; } = string.Empty;

    public bool IsActive { get; private set; } = true;

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }

    public DateTime? DeletedAt { get; init; }
}