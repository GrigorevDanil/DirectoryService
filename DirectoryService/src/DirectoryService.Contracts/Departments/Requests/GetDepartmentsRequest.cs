namespace DirectoryService.Contracts.Departments.Requests;

/// <summary>
/// Запрос на получение подразделений
/// </summary>
/// <param name="Search">Поиск по названию</param>
/// <param name="LocationIds">Поиск по локациям</param>
/// <param name="IsActive">Активность</param>
/// <param name="IsParent">Родитель</param>
public record GetDepartmentsRequest(
    string? Search,
    Guid[]? LocationIds,
    bool? IsActive,
    bool? IsParent,
    string SortBy = "name",
    string SortDirection = "asc") : PaginationRequest;