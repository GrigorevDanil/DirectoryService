using DirectoryService.Contracts.DepartmentLocation.Dtos;

namespace DirectoryService.Contracts.Departments.Dtos;

public record DepartmentDtoOnlyWithPath
{
    public Guid Id { get; init; }

    public string Path { get; init; } = string.Empty;

    public int Depth { get; init; }
}