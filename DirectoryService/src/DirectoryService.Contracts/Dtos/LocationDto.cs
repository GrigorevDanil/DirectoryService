namespace DirectoryService.Contracts.Dtos;

public record LocationDto(
    string Name,
    AddressDto Address,
    string Timezone);