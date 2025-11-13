using CSharpFunctionalExtensions;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Locations.ValueObjects;
using Shared;

namespace DirectoryService.Application.Locations;

public interface ILocationRepository
{
    public Task<Guid> AddLocationAsync(Location location, CancellationToken cancellationToken = default);

    public Task<UnitResult<Error>> DeleteUnusedLocationsByDepartmentIdAsync(DepartmentId id, CancellationToken cancellationToken = default);

    public Task<Result<Location, Error>> GetLocationByIdAsync(LocationId id, CancellationToken cancellationToken = default);

    public Task<UnitResult<Errors>> CheckExistingAndActiveIds(Guid[] ids, CancellationToken cancellationToken = default);
}