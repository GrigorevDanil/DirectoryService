using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.Locations.ValueObjects;

/// <summary>
/// Адрес
/// </summary>
public record Address
{
    public const int MAX_LENGTH_COUNTRY = 100;

    public const int MAX_LENGTH_POSTAL_CODE = 6;

    public const int MAX_LENGTH_REGION = 100;

    public const int MAX_LENGTH_CITY = 100;

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
    /// Создает новый объект "Адрес"
    /// </summary>
    /// <param name="country">Страна.</param>
    /// <param name="postalCode">Почтовый индекс.</param>
    /// <param name="region">Регион.</param>
    /// <param name="city">Город.</param>
    /// <param name="street">Улица.</param>
    /// <param name="houseNumber">Номер дома.</param>
    /// <returns>Новый объект или ошибка.</returns>
    public static Result<Address> Of(
        string country,
        string postalCode,
        string region,
        string city,
        string street,
        string houseNumber)
    {
        if (string.IsNullOrWhiteSpace(country) || country.Length < MAX_LENGTH_COUNTRY)
            return Result.Failure<Address>("Country is empty or does not match the allowed length");

        if (string.IsNullOrWhiteSpace(postalCode) || postalCode.Length < MAX_LENGTH_POSTAL_CODE || int.TryParse(postalCode, out _))
            return Result.Failure<Address>("Postal code may be empty, invalid in length, or not a number");

        if (string.IsNullOrWhiteSpace(region) || region.Length < MAX_LENGTH_REGION)
            return Result.Failure<Address>("Region is empty or does not match the allowed length");

        if (string.IsNullOrWhiteSpace(city) || city.Length < MAX_LENGTH_CITY)
            return Result.Failure<Address>("City is empty or does not match the allowed length");

        if (string.IsNullOrWhiteSpace(street) || street.Length < MAX_LENGTH_STREET)
            return Result.Failure<Address>("Street is empty or does not match the allowed length");

        if (string.IsNullOrWhiteSpace(houseNumber) || houseNumber.Length < MAX_LENGTH_HOUSE_NUMBER ||
            int.TryParse(houseNumber, out int parsedHouseNumber) || parsedHouseNumber < 1)
            return Result.Failure<Address>("House number may be empty, longer than allowed, not a number, or less than 1");

        return new Address(country, postalCode, region, city, street, houseNumber);
    }
}