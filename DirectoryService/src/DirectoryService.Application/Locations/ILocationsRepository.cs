using CSharpFunctionalExtensions;
using DirectoryService.Domain.Locations;
using Shared;

namespace DirectoryService.Application.Locations;

/// <summary>
/// Интерфейс репозиторий для сущности локации
/// </summary>
public interface ILocationsRepository
{
    /// <summary>
    /// Создать локацию ассинхронно
    /// </summary>
    /// <param name="location">Сущность локация.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданной локации или ошибку <see cref="Error"/>.</returns>
    public Task<Result<Guid, Error>> AddLocationAsync(Location location, CancellationToken cancellationToken);
}