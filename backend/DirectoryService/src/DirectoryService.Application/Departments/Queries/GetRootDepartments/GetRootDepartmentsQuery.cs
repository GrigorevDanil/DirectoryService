using DirectoryService.Contracts.Departments.Requests;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Departments.Queries.GetRootDepartments;

public record GetRootDepartmentsQuery(GetRootDepartmentsRequest Request) : IQuery;