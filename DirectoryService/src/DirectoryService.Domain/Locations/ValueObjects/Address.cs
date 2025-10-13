using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Dtos;
using Shared;

namespace DirectoryService.Domain.Locations.ValueObjects;

/// <summary>
/// Адрес
/// </summary>
public record Address
{
    public const int MIN_LENGTH_COUNTRY = 3;
    public const int MAX_LENGTH_COUNTRY = 100;

    public const int LENGTH_POSTAL_CODE = 6;

    public const int MIN_LENGTH_REGION = 3;
    public const int MAX_LENGTH_REGION = 100;

    public const int MIN_LENGTH_CITY = 3;
    public const int MAX_LENGTH_CITY = 100;

    public const int MIN_LENGTH_STREET = 3;
    public const int MAX_LENGTH_STREET = 150;

    public const int MAX_LENGTH_HOUSE_NUMBER = 3;

    /// <summary>
    /// Страна
    /// </summary>
    public string Country { get; }

    /// <summary>
    /// Почтовый индекс
    /// </summary>
    public string PostalCode { get; }

    /// <summary>
    /// Регион
    /// </summary>
    public string Region { get; }

    /// <summary>
    /// Город
    /// </summary>
    public string City { get; }

    /// <summary>
    /// Улица
    /// </summary>
    public string Street { get; }

    /// <summary>
    /// Номер дома
    /// </summary>
    public string HouseNumber { get; }

    private Address(
        string country,
        string postalCode,
        string region,
        string city,
        string street,
        string houseNumber)
    {
        Country = country;
        PostalCode = postalCode;
        Region = region;
        City = city;
        Street = street;
        HouseNumber = houseNumber;
    }

    /// <summary>
    /// Создает новый объект <see cref="Address"/>
    /// </summary>
    /// <param name="country">Страна.</param>
    /// <param name="postalCode">Почтовый индекс.</param>
    /// <param name="region">Регион.</param>
    /// <param name="city">Город.</param>
    /// <param name="street">Улица.</param>
    /// <param name="houseNumber">Номер дома.</param>
    /// <returns>Новый объект <see cref="Address"/> или список ошибок <see cref="Errors"/>.</returns>
    public static Result<Address, Errors> Of(
        string country,
        string postalCode,
        string region,
        string city,
        string street,
        string houseNumber)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(country) || country.Length > MAX_LENGTH_COUNTRY || country.Length < MIN_LENGTH_COUNTRY)
            errors.Add(GeneralErrors.ValueIsEmptyOrInvalidLength("location.address"));

        if (string.IsNullOrWhiteSpace(postalCode) || postalCode.Length != LENGTH_POSTAL_CODE || !int.TryParse(postalCode, out _))
            errors.Add(GeneralErrors.ValueIsEmptyOrInvalidLength("location.postalCode", "Postal code may be empty, invalid in length, or not a number"));

        if (string.IsNullOrWhiteSpace(region) || region.Length > MAX_LENGTH_REGION || region.Length < MIN_LENGTH_REGION)
            errors.Add(GeneralErrors.ValueIsEmptyOrInvalidLength("location.region"));

        if (string.IsNullOrWhiteSpace(city) || city.Length > MAX_LENGTH_CITY || region.Length < MIN_LENGTH_CITY)
            errors.Add(GeneralErrors.ValueIsEmptyOrInvalidLength("location.city"));

        if (string.IsNullOrWhiteSpace(street) || street.Length > MAX_LENGTH_STREET || region.Length < MIN_LENGTH_STREET)
            errors.Add(GeneralErrors.ValueIsEmptyOrInvalidLength("location.street"));

        if (string.IsNullOrWhiteSpace(houseNumber) || houseNumber.Length > MAX_LENGTH_HOUSE_NUMBER ||
            !int.TryParse(houseNumber, out int parsedHouseNumber) || parsedHouseNumber < 1)
            errors.Add(GeneralErrors.ValueIsEmptyOrInvalidLength("location.houseNumber", "House number may be empty, longer than allowed, not a number, or less than 1"));

        if (errors.Count != 0)
            return new Errors(errors);

        return new Address(country, postalCode, region, city, street, houseNumber);
    }

    /// <summary>
    /// Создает новый объект <see cref="Address"/>
    /// </summary>
    /// <param name="addressDto">Входящие данные об адресе.</param>
    /// <returns>Новый объект <see cref="Address"/> или список ошибок <see cref="Errors"/>.</returns>
    public static Result<Address, Errors> Of(
        AddressDto addressDto)
    {
        return Of(
            addressDto.Country,
            addressDto.PostalCode,
            addressDto.Region,
            addressDto.City,
            addressDto.Street,
            addressDto.HouseNumber);
    }
}