using CSharpFunctionalExtensions;
using DirectoryService.Domain.Locations.ValueObjects;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Locations;

/// <summary>
/// Сущность локации
/// </summary>
public class Location: BaseEntity<LocationId>, ISoftDeletable
{
    public Location(
        LocationId id,
        LocationName name,
        Timezone timezone,
        Address address)
    {
        Id = id;
        Name = name;
        Timezone = timezone;
        Address = address;
    }

    public LocationName Name { get; private set; }

    public Timezone Timezone { get; private set; }

    public Address Address { get; private set; }

    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Переименовать название локации
    /// </summary>
    /// <param name="name">Новое название локации.</param>
    /// <returns>Результат выполнения переименования.</returns>
    public Result Rename(string name)
    {
        var nameResult = LocationName.Of(name);

        if (nameResult.IsFailure)
            return Result.Failure(nameResult.Error);

        Name = nameResult.Value;

        return Result.Success();
    }

    /// <summary>
    /// Изменяет часовой пояс
    /// </summary>
    /// <param name="timezone">Новый IANA-код часового пояса.</param>
    /// <returns>Результат выполнения изменения часового пояса.</returns>
    public Result ChangeTimezone(string timezone)
    {
        var timezoneResult = Timezone.Of(timezone);

        if (timezoneResult.IsFailure)
            return Result.Failure(timezoneResult.Error);

        Timezone = timezoneResult.Value;

        return Result.Success();
    }

    /// <summary>
    /// Изменяет адрес локации
    /// </summary>
    /// <param name="country">Страна.</param>
    /// <param name="postalCode">Почтовый индекс.</param>
    /// <param name="region">Регион.</param>
    /// <param name="city">Город.</param>
    /// <param name="street">Улица.</param>
    /// <param name="houseNumber">Номер дома.</param>
    /// <returns>Результат выполнения изменения адреса.</returns>
    public Result ChangeAddress(
        string country,
        string postalCode,
        string region,
        string city,
        string street,
        int houseNumber)
    {
        var addressResult = Address.Of(
            country,
            postalCode,
            region,
            city,
            street,
            houseNumber);

        if (addressResult.IsFailure)
            return Result.Failure(addressResult.Error);

        Address = addressResult.Value;

        return Result.Success();
    }

    /// <summary>
    /// Помечает сущность как удаленную
    /// </summary>
    public void MarkAsDelete()
    {
        IsActive = false;
    }
}