namespace DirectoryService.Contracts.Locations.Requests;

/// <summary>
/// Запрос на получение локаций
/// </summary>
/// <param name="Search">Поиск по названию</param>
/// <param name="DepartmentIds">Идентификаторы подразделений</param>
/// <param name="IsActive">Активность</param>
public record GetLocationsRequest(
    string? Search,
    Guid[]? DepartmentIds,
    bool? IsActive,
    string SortBy = "name",
    string SortDirection = "asc") : PaginationRequest;