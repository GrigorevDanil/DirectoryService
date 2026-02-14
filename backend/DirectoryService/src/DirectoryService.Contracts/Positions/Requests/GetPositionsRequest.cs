namespace DirectoryService.Contracts.Positions.Requests;

/// <summary>
/// Запрос на получение позиций
/// </summary>
/// <param name="Search">Поиск по названию</param>
/// <param name="DepartmentIds">Идентификаторы подразделений</param>
/// <param name="IsActive">Активность</param>
public record GetPositionsRequest(
    string? Search,
    Guid[]? DepartmentIds,
    bool? IsActive,
    string SortBy = "name",
    string SortDirection = "asc") : PaginationRequest;