namespace DirectoryService.Contracts.Dtos;

public record AddressDto(
    string Country,
    string PostalCode,
    string Region,
    string City,
    string Street,
    string HouseNumber);