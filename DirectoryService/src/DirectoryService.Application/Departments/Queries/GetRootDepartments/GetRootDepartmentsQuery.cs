using DirectoryService.Contracts.Departments.Requests;
using Shared.Abstractions;

namespace DirectoryService.Application.Departments.Queries.GetRootDepartments;

public record GetRootDepartmentsQuery(GetRootDepartmentsRequest Request) : IQuery;