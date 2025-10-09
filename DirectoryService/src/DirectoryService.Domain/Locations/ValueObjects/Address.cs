using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.Locations.ValueObjects;

/// <summary>
/// Адрес
/// </summary>
public record Address
{
    public const int POSTAL_CODE_LENGTH = 6;

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
    public int HouseNumber { get; }

    private Address(
        string country,
        string postalCode,
        string region,
        string city,
        string street,
        int houseNumber)
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
        int houseNumber)
    {
        if (string.IsNullOrWhiteSpace(country))
            return Result.Failure<Address>("Country cannot be empty.");

        if (string.IsNullOrWhiteSpace(postalCode) || postalCode.Length < POSTAL_CODE_LENGTH)
            return Result.Failure<Address>("Postal code cannot be empty or less than 6.");

        if (string.IsNullOrWhiteSpace(region))
            return Result.Failure<Address>("Region cannot be empty.");

        if (string.IsNullOrWhiteSpace(city))
            return Result.Failure<Address>("City cannot be empty.");

        if (string.IsNullOrWhiteSpace(street))
            return Result.Failure<Address>("Street cannot be empty.");

        if (houseNumber < 1)
            return Result.Failure<Address>("House number cannot be less than 1.");

        return new Address(country, postalCode, region, city, street, houseNumber);
    }
}