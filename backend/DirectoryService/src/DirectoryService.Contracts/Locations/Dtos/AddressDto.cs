using SharedService.SharedKernel;

namespace DirectoryService.Contracts.Locations.Dtos;

public record AddressDto(
    string Country,
    string PostalCode,
    string Region,
    string City,
    string Street,
    string HouseNumber) : IDapperJson;