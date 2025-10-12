using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Dtos;
using Shared;

namespace DirectoryService.Application.Locations;

/// <summary>
/// Интерфейс сервиса для сущности локации
/// </summary>
public interface ILocationsService
{
    /// <summary>
    /// Создать локацию ассинхронно
    /// </summary>
    /// <param name="locationDto">Входящие данные о локации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданной локации или список ошибок <see cref="Errors"/>.</returns>
    public Task<Result<Guid, Errors>> AddAsync(LocationDto locationDto, CancellationToken cancellationToken);
}