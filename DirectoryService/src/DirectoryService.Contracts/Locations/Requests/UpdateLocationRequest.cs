using DirectoryService.Contracts.Locations.Dtos;

namespace DirectoryService.Contracts.Locations.Requests;

public record UpdateLocationRequest(string Name, string Timezone, AddressDto Address);