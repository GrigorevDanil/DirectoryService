using CSharpFunctionalExtensions;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Locations.ValueObjects;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Locations;

public interface ILocationRepository
{
    Task<Guid> AddLocationAsync(Location location, CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> DeleteUnusedLocationsByDepartmentIdAsync(DepartmentId id, CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> DeleteOutdatedLocationsAsync(CancellationToken cancellationToken = default);

    Task<Result<Location, Error>> GetLocationByIdAsync(LocationId id, CancellationToken cancellationToken = default);

    Task<UnitResult<Errors>> CheckExistingAndActiveIds(Guid[] ids, CancellationToken cancellationToken = default);
}