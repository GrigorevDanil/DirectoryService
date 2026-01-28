namespace DirectoryService.Contracts.Departments.Dtos;

public record DepartmentShortDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Identifier { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}