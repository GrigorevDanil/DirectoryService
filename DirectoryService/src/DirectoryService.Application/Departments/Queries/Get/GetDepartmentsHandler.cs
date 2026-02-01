using System.Data;
using System.Data.Common;
using Dapper;
using DirectoryService.Contracts.Departments.Dtos;
using SharedService.Core.Database;
using SharedService.Core.Handlers;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Departments.Queries.Get;

public class GetDepartmentsHandler : IQueryHandler<GetDepartmentsQuery, PaginationEnvelope<DepartmentShortDto>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetDepartmentsHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<PaginationEnvelope<DepartmentShortDto>> Handle(
        GetDepartmentsQuery query,
        CancellationToken cancellationToken = new CancellationToken())
    {
        List<string> conditionals = [];
        DynamicParameters parameters = new();

        if (query.Request.LocationIds is { Length: > 0 })
        {
            parameters.Add("LocationIds", query.Request.LocationIds, DbType.Object);
            conditionals.Add("""
                             EXISTS (
                                 SELECT 1
                                 FROM department_locations dl
                                 WHERE dl.department_id = d.id
                                   AND dl.location_id = ANY(@LocationIds)
                             )
                             """);
        }

        if (!string.IsNullOrWhiteSpace(query.Request.Search))
        {
            parameters.Add("Search", query.Request.Search, DbType.String);
            conditionals.Add(" CONCAT(d.name, d.identifier) ILIKE '%' || @Search || '%' ");
        }

        if (query.Request.IsActive is not null)
        {
            parameters.Add("IsActive", query.Request.IsActive, DbType.Boolean);
            conditionals.Add(" d.is_active = @IsActive ");
        }

        if (query.Request.IsParent is not null)
        {
            conditionals.Add(
                " (SELECT COUNT(*) FROM departments WHERE parent_id = d.id) " +
                (query.Request.IsParent.Value ? " > 0 " : " = 0 "));
        }

        if (query.Request.ParentId is not null)
        {
            parameters.Add("ParentId", query.Request.ParentId, DbType.Guid);
            conditionals.Add(" d.parent_id = @ParentId ");
        }

        parameters.Add("PageSize", query.Request.PageSize, DbType.Int32);
        parameters.Add("Page", (query.Request.Page - 1) * query.Request.PageSize, DbType.Int32);

        string conditionalsString =
            conditionals.Count != 0 ? "WHERE " + string.Join(" AND ", conditionals) : string.Empty;

        string sortDirection = query.Request.SortDirection.ToLowerInvariant() == "asc" ? "ASC" : "DESC";

        string sortBy = query.Request.SortBy.ToLowerInvariant() switch
        {
            "name" => "d.name",
            "path" => "d.path",
            "createdat" => "d.created_at",
            _ => "d.name"
        };

        string orderByString = $"ORDER BY {sortBy} {sortDirection}";

        long? totalCount = null;

        string sql = $"""
                      SELECT d.*, COUNT(*) OVER() as totalCount FROM departments d
                      {conditionalsString}
                      {orderByString}
                      LIMIT @PageSize OFFSET @Page
                      """;

        DbConnection dbConnection = _dbConnectionFactory.GetDbConnection();

        IEnumerable<DepartmentShortDto> res = await dbConnection.QueryAsync<DepartmentShortDto, long, DepartmentShortDto>(
            sql,
            param: parameters,
            splitOn: "totalCount",
            map: (l, tc) =>
            {
                totalCount ??= tc;
                return l;
            });

        return new PaginationEnvelope<DepartmentShortDto>(res, totalCount ?? 0);
    }
}