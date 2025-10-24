namespace DirectoryService.Contracts.Locations.Dtos;

public record LocationDto
{
    public Guid Id { get; init; }

    public string Name { get; private set; } = string.Empty;

    public string Timezone { get; private set; } = string.Empty;

    public required AddressDto Address { get; init; }

    public bool IsActive { get; private set; } = true;

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}