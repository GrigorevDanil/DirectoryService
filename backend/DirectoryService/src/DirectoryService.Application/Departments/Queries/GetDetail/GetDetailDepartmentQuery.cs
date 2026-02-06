using SharedService.Core.Handlers;

namespace DirectoryService.Application.Departments.Queries.GetDetail;

public record GetDetailDepartmentQuery(Guid Id):IQuery;