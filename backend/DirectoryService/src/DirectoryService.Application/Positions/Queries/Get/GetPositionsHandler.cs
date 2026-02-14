using System.Data;
using System.Data.Common;
using Dapper;
using DirectoryService.Contracts.Positions.Dtos;
using SharedService.Core.Database;
using SharedService.Core.Handlers;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Positions.Queries.Get;

public class GetPositionsHandler : IQueryHandler<GetPositionsQuery, PaginationEnvelope<PositionDto>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetPositionsHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<PaginationEnvelope<PositionDto>> Handle(
        GetPositionsQuery query,
        CancellationToken cancellationToken = new CancellationToken())
    {
        List<string> conditionals = [];
        DynamicParameters parameters = new();

        if (query.Request.DepartmentIds is { Length: > 0 })
        {
            parameters.Add("DepartmentIds", query.Request.DepartmentIds, DbType.Object);
            conditionals.Add("""
                             EXISTS (
                                 SELECT 1
                                 FROM department_positions dp
                                 WHERE dp.position_id = p.id
                                   AND dp.department_id = ANY(@DepartmentIds)
                             )
                             """);
        }

        if (!string.IsNullOrWhiteSpace(query.Request.Search))
        {
            parameters.Add("Search", query.Request.Search, DbType.String);
            conditionals.Add(" p.name ILIKE '%' || @Search || '%' ");
        }

        if (query.Request.IsActive != null)
        {
            parameters.Add("IsActive", query.Request.IsActive, DbType.Boolean);
            conditionals.Add(" p.is_active = @IsActive ");
        }

        parameters.Add("PageSize", query.Request.PageSize, DbType.Int32);
        parameters.Add("Page", (query.Request.Page - 1) * query.Request.PageSize, DbType.Int32);

        string conditionalsString =
            conditionals.Count != 0 ? "WHERE " + string.Join(" AND ", conditionals) : string.Empty;

        string sortDirection = query.Request.SortDirection.ToLowerInvariant() == "asc" ? "ASC" : "DESC";

        string sortBy = query.Request.SortBy.ToLowerInvariant() switch
        {
            "name" => "p.name",
            "countdepartments" => "count_departments",
            "createdat" => "p.created_at",
            _ => "l.name"
        };

        string orderByString = $"ORDER BY {sortBy} {sortDirection}";

        long? totalCount = null;

        string sql = $"""
                      SELECT p.*,(SELECT COUNT(*) FROM department_positions dp WHERE dp.position_id = p.id) as count_departments, COUNT(*) OVER() as totalCount FROM positions p
                      {conditionalsString}
                      {orderByString}
                      LIMIT @PageSize OFFSET @Page
                      """;

        DbConnection dbConnection = _dbConnectionFactory.GetDbConnection();

        IEnumerable<PositionDto> res = await dbConnection.QueryAsync<PositionDto, long, PositionDto>(
            sql,
            param: parameters,
            splitOn: "totalCount",
            map: (l, tc) =>
            {
                totalCount ??= tc;
                return l;
            });

        return new PaginationEnvelope<PositionDto>(res, totalCount ?? 0);
    }
}