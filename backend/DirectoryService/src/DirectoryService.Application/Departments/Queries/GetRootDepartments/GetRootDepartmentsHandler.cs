using System.Globalization;
using Dapper;
using DirectoryService.Application.Constants;
using DirectoryService.Contracts.Departments.Dtos;
using Microsoft.Extensions.Caching.Distributed;
using SharedService.Core.Caching;
using SharedService.Core.Database;
using SharedService.Core.Handlers;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Departments.Queries.GetRootDepartments;

public class GetRootDepartmentsHandler : IQueryHandler<GetRootDepartmentsQuery, PaginationEnvelope<DepartmentDto>>
{
    private readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
    };

    private readonly IDbConnectionFactory _dbConnectionFactory;

    private readonly ICacheService _cache;

    public GetRootDepartmentsHandler(IDbConnectionFactory dbConnectionFactory, ICacheService cache)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _cache = cache;
    }

    public async Task<PaginationEnvelope<DepartmentDto>> Handle(
        GetRootDepartmentsQuery query,
        CancellationToken cancellationToken = default)
    {
        string key = CachingKeys.CreateDepartmentsKey(
            query.Request.Prefetch.ToString(CultureInfo.InvariantCulture),
            query.Request.Page.ToString(CultureInfo.InvariantCulture),
            query.Request.PageSize.ToString(CultureInfo.InvariantCulture));

        return await _cache.GetOrSetAsync(
            key,
            async () => await GetRootDepartments(query),
            _cacheOptions,
            cancellationToken) ?? new PaginationEnvelope<DepartmentDto>([], 0);
    }

    private async Task<PaginationEnvelope<DepartmentDto>?> GetRootDepartments(GetRootDepartmentsQuery query)
    {
        const string sql = """
                           WITH roots AS (
                           SELECT COUNT(*) OVER() as totalCount,
                                  d.id,
                                  d.parent_id,
                                  d.name,
                                  d.identifier,
                                  d.path,
                                  d.depth,
                                  d.is_active,
                                  d.created_at,
                                  d.updated_at,
                                  d.deleted_at
                           FROM departments d
                           WHERE d.parent_id IS NULL
                           ORDER BY d.created_at
                           LIMIT @RootSize
                           OFFSET @RootPage)

                           SELECT *,
                                  (EXISTS(
                                      SELECT 1
                                      FROM departments d
                                      WHERE d.parent_id = r.id
                                      OFFSET @ChildSize
                                  )) AS has_more_children
                           FROM roots r

                           UNION ALL

                           SELECT 0,
                                  c.*,
                                  (EXISTS(
                                      SELECT 1
                                      FROM departments d
                                      WHERE d.parent_id = c.id
                                  )) AS has_more_children

                           FROM roots r
                           CROSS JOIN LATERAL (
                               SELECT d.id,
                                      d.parent_id,
                                      d.name,
                                      d.identifier,
                                      d.path,
                                      d.depth,
                                      d.is_active,
                                      d.created_at,
                                      d.updated_at,
                                      d.deleted_at
                               FROM departments d
                               WHERE d.parent_id = r.id AND d.is_active = true
                               ORDER BY d.created_at
                               LIMIT @ChildSize
                               ) AS c
                           """;

        var dbConnection = _dbConnectionFactory.GetDbConnection();

        long? totalCount = null;

        var departmentRows = (await dbConnection.QueryAsync<long, DepartmentDto, DepartmentDto>(
            sql,
            param: new
            {
                RootSize = query.Request.PageSize,
                RootPage = (query.Request.Page - 1) * query.Request.PageSize,
                ChildSize = query.Request.Prefetch,
            },
            map: (t, d) =>
            {
                totalCount ??= t;
                return d;
            })).ToList();

        var departmentDictionary = departmentRows.ToDictionary(x => x.Id);
        List<DepartmentDto> roots = [];

        foreach (var row in departmentRows)
        {
            var department = departmentDictionary[row.Id];

            if (row.ParentId.HasValue
                && departmentDictionary.TryGetValue(row.ParentId.Value, out var parent))
            {
                parent.Children.Add(department);
            }
            else
            {
                roots.Add(department);
            }
        }

        return new PaginationEnvelope<DepartmentDto>(roots, totalCount ?? 0);
    }
}