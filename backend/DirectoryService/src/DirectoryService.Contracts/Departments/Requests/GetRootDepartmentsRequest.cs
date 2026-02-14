namespace DirectoryService.Contracts.Departments.Requests;

public record GetRootDepartmentsRequest(int Prefetch = 3) : PaginationRequest;