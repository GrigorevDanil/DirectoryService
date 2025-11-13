using CSharpFunctionalExtensions;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Positions;
using Shared;

namespace DirectoryService.Application.Positions;

public interface IPositionsRepository
{
    public Task<Guid> AddPositionAsync(Position position, CancellationToken cancellationToken = default);

    public Task<UnitResult<Error>> DeleteUnusedPositionsByDepartmentIdAsync(
        DepartmentId id,
        CancellationToken cancellationToken = default);
}