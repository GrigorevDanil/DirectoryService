using DirectoryService.Contracts.Locations.Dtos;

namespace DirectoryService.Contracts.Locations.Requests;

public record CreateLocationRequest(
    string Name,
    AddressDto Address,
    string Timezone);