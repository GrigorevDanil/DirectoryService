using CSharpFunctionalExtensions;
using DirectoryService.Domain.Locations.ValueObjects;
using Shared;

namespace DirectoryService.Domain.Locations;

/// <summary>
/// Сущность локации
/// </summary>
public sealed class Location: BaseEntity<LocationId>, ISoftDeletable
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

    /// <summary>
    /// Конструктор для работы EF
    /// </summary>
    private Location() { }

    public LocationName Name { get; private set; } = null!;

    public Timezone Timezone { get; private set; } = null!;

    public Address Address { get; private set; } = null!;

    public bool IsActive { get; private set; } = true;

    public DateTime? DeletedAt { get;  private set; }

    /// <summary>
    /// Переименовать название локации
    /// </summary>
    /// <param name="name">Новое название локации.</param>
    /// <returns>Результат выполнения переименования.</returns>
    public UnitResult<Error> Rename(string name)
    {
        var nameResult = LocationName.Of(name);

        if (nameResult.IsFailure)
            return nameResult.Error;

        Name = nameResult.Value;

        return Result.Success<Error>();
    }

    /// <summary>
    /// Изменяет часовой пояс
    /// </summary>
    /// <param name="timezone">Новый IANA-код часового пояса.</param>
    /// <returns>Результат выполнения изменения часового пояса.</returns>
    public UnitResult<Error> ChangeTimezone(string timezone)
    {
        var timezoneResult = Timezone.Of(timezone);

        if (timezoneResult.IsFailure)
            return timezoneResult.Error;

        Timezone = timezoneResult.Value;

        return Result.Success<Error>();
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
    public UnitResult<Errors> ChangeAddress(
        string country,
        string postalCode,
        string region,
        string city,
        string street,
        string houseNumber)
    {
        var addressResult = Address.Of(
            country,
            postalCode,
            region,
            city,
            street,
            houseNumber);

        if (addressResult.IsFailure)
            return addressResult.Error;

        Address = addressResult.Value;

        return Result.Success<Errors>();
    }

    /// <summary>
    /// Помечает сущность как удаленную
    /// </summary>
    public void MarkAsDelete()
    {
        IsActive = false;
        DeletedAt = DateTime.UtcNow;
    }
}