namespace DirectoryService.Contracts;

/// <summary>
/// Запрос пагинации
/// </summary>
/// <param name="Page">Страница</param>
/// <param name="PageSize">Количество записей</param>
public abstract record PaginationRequest(
    int Page = 1,
    int PageSize = 20);