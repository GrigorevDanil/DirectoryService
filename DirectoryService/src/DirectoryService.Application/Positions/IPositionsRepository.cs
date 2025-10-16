using CSharpFunctionalExtensions;
using DirectoryService.Domain.Positions;
using Shared;

namespace DirectoryService.Application.Positions;

public interface IPositionsRepository
{
    public Task<Result<Guid, Error>> AddPositionAsync(Position position, CancellationToken cancellationToken = default);
}