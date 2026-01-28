using DirectoryService.Contracts.Positions.Requests;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Positions.Queries.Get;

public record GetPositionsQuery(GetPositionsRequest Request): IQuery;