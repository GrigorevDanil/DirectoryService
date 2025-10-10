using DirectoryService.Domain.Locations;

namespace DirectoryService.Application.Locations;

/// <summary>
/// Интерфейс репозиторий для сущности локации
/// </summary>
public interface ILocationRepository
{
    /// <summary>
    /// Создать локацию ассинхронно
    /// </summary>
    /// <param name="location">Сущность локация.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданной локации.</returns>
    public Task<Guid> AddLocationAsync(Location location, CancellationToken cancellationToken);
}