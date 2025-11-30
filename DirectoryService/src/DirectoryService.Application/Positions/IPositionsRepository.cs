using CSharpFunctionalExtensions;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Positions;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Positions;

public interface IPositionsRepository
{
    Task<Guid> AddPositionAsync(Position position, CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> DeleteUnusedPositionsByDepartmentIdAsync(
        DepartmentId id,
        CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> DeleteOutdatedPositionsAsync(CancellationToken cancellationToken = default);
}