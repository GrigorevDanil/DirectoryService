using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Dtos;

namespace DirectoryService.Application.Abstractions.Locations;

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
    /// <returns>Идентификатор созданной локации.</returns>
    public Task<Result<Guid>> AddAsync(LocationDto locationDto, CancellationToken cancellationToken);
}