using SharedService.Core.Handlers;

namespace DirectoryService.Application.Positions.Queries.GetDetail;

public record GetDetailPositionQuery(Guid Id) : IQuery;