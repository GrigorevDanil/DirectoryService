using DirectoryService.Domain.Locations;

namespace DirectoryService.Application.Abstractions.Locations;

/// <summary>
/// Интерфейс репозиторий для сущности локации
/// </summary>
public interface ILocationRepository
{
    /// <summary>
    /// Создать локацию ассинхронно
    /// </summary>
    /// <param name="location">Сущность локация.</param>
    /// <returns>Идентификатор созданной локации.</returns>
    public Task<Guid> AddLocationAsync(Location location);
}