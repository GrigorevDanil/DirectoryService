using System.Data;
using Dapper;
using DirectoryService.Contracts.Locations.Dtos;
using Shared;
using Shared.Abstractions;
using Shared.Database;

namespace DirectoryService.Application.Locations.Queries.Get;

public class GetLocationsHandler : IQueryHandler<GetLocationsQuery, PaginationEnvelope<LocationDto>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetLocationsHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<PaginationEnvelope<LocationDto>> Handle(
        GetLocationsQuery query,
        CancellationToken cancellationToken = default)
    {
        List<string> conditionals = [];
        var parameters = new DynamicParameters();

        if (query.Request.DepartmentIds is { Length: > 0 })
        {
            parameters.Add("DepartmentIds", query.Request.DepartmentIds, DbType.Object);
            conditionals.Add("""
                             EXISTS (
                                 SELECT 1
                                 FROM department_locations dl
                                 WHERE dl.location_id = l.id
                                   AND dl.department_id = ANY(@DepartmentIds)
                             )
                             """);
        }

        if (!string.IsNullOrWhiteSpace(query.Request.Search))
        {
            parameters.Add("Search", query.Request.Search, DbType.String);
            conditionals.Add(" l.name ILIKE '%' || @Search || '%' ");
        }

        if (query.Request.IsActive != null)
        {
            parameters.Add("IsActive", query.Request.IsActive, DbType.Boolean);
            conditionals.Add(" l.is_active = @IsActive ");
        }

        parameters.Add("PageSize", query.Request.PageSize, DbType.Int32);
        parameters.Add("Page", (query.Request.Page - 1) * query.Request.PageSize, DbType.Int32);

        string conditionalsString =
            conditionals.Count != 0 ? "WHERE " + string.Join(" AND ", conditionals) : string.Empty;

        string sortDirection = query.Request.SortDirection.ToLower() == "asc" ? "ASC" : "DESC";

        string sortBy = query.Request.SortBy.ToLower() switch
        {
            "name" => "l.name",
            "createdat" => "l.created_at",
            _ => "l.name"
        };

        string orderByString = $"ORDER BY {sortBy} {sortDirection}";

        long? totalCount = null;

        string sql = $"""
                      SELECT l.*, COUNT(*) OVER() as totalCount FROM locations l
                      {conditionalsString}
                      {orderByString}
                      LIMIT @PageSize OFFSET @Page
                      """;

        var dbConnection = _dbConnectionFactory.GetDbConnection();

        var res = await dbConnection.QueryAsync<LocationDto, long, LocationDto>(
            sql,
            param: parameters,
            splitOn: "totalCount",
            map: (l, tc) =>
            {
                totalCount ??= tc;
                return l;
            });

        return new PaginationEnvelope<LocationDto>(res, totalCount ?? 0);
    }
}