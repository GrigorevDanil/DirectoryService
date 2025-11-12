using DirectoryService.Contracts.Departments.Requests;
using Shared.Abstractions;

namespace DirectoryService.Application.Departments.Queries.GetChildrenDepartments;

public record GetChildrenDepartmentsQuery(Guid ParentId, GetChildrenDepartmentsRequest Request) : IQuery;