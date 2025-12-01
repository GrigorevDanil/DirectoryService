using DirectoryService.Contracts.Locations.Requests;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Locations.Queries.Get;

public record GetLocationsQuery(GetLocationsRequest Request) : IQuery;