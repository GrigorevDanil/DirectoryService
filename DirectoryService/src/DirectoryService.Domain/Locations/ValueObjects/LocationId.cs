namespace DirectoryService.Domain.Locations.ValueObjects;

/// <summary>
/// Уникальный идентификатор локации
/// </summary>
public record LocationId
{
    private LocationId(Guid value) => Value = value;

    public Guid Value { get; private set; }

    /// <summary>
    /// Создание нового идентификатора для локации
    /// </summary>
    /// <returns>Новый идентификатор локации.</returns>
    public static LocationId Create() => new(Guid.NewGuid());

    /// <summary>
    /// Создание идентификатора локации из входящего идентификатора
    /// </summary>
    /// <param name="locationId">Входящий идентификатор.</param>
    /// <returns>Идентификатор локации.</returns>
    public static LocationId Of(Guid locationId) => new(locationId);

    /// <summary>
    /// Создание идентификаторов локаций из входящих идентификаторов
    /// </summary>
    /// <param name="locationIds">Входящие идентификаторы.</param>
    /// <returns>Идентификаторы локаций.</returns>
    public static LocationId[] Of(Guid[] locationIds) => locationIds.Select(Of).ToArray();
};