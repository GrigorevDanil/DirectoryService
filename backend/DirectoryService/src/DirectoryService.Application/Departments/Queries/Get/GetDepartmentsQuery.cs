using DirectoryService.Contracts.Departments.Requests;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Departments.Queries.Get;

public record GetDepartmentsQuery(GetDepartmentsRequest Request) : IQuery;