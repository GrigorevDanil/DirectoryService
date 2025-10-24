using DirectoryService.Contracts.Locations.Requests;
using Shared.Abstractions;

namespace DirectoryService.Application.Locations.Queries.Get;

public record GetLocationsQuery(GetLocationsRequest Request) : IQuery;