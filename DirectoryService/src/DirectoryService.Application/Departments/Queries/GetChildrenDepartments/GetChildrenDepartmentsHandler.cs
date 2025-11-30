using System.Globalization;
using Dapper;
using DirectoryService.Application.Constants;
using DirectoryService.Contracts.Departments.Dtos;
using Microsoft.Extensions.Caching.Distributed;
using SharedService.Core.Caching;
using SharedService.Core.Database;
using SharedService.Core.Handlers;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Departments.Queries.GetChildrenDepartments;

public class GetChildrenDepartmentsHandler : IQueryHandler<GetChildrenDepartmentsQuery, PaginationEnvelope<DepartmentDto>>
{
    private readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
    };

    private readonly IDbConnectionFactory _dbConnectionFactory;

    private readonly ICacheService _cache;

    public GetChildrenDepartmentsHandler(IDbConnectionFactory dbConnectionFactory, ICacheService cache)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _cache = cache;
    }

    public async Task<PaginationEnvelope<DepartmentDto>> Handle(
        GetChildrenDepartmentsQuery query,
        CancellationToken cancellationToken = default)
    {
        string key = CachingKeys.CreateDepartmentsKey(
            query.ParentId.ToString(),
            query.Request.Page.ToString(CultureInfo.InvariantCulture),
            query.Request.PageSize.ToString(CultureInfo.InvariantCulture));

        return await _cache.GetOrSetAsync(
            key,
            async () => await GetChildrenDepartments(query),
            _cacheOptions,
            cancellationToken) ?? new PaginationEnvelope<DepartmentDto>([], 0);
    }

    private async Task<PaginationEnvelope<DepartmentDto>?> GetChildrenDepartments(GetChildrenDepartmentsQuery query)
    {
        const string sql = """
                           SELECT cd.id,
                                  cd.parent_id,
                                  cd.name,
                                  cd.identifier,
                                  cd.path,
                                  cd.depth,
                                  cd.is_active,
                                  cd.created_at,
                                  cd.updated_at,
                                  (EXISTS(
                                      SELECT 1
                                      FROM departments d
                                      WHERE d.parent_id = cd.id
                                  )) AS has_more_children,
                                  COUNT(*) OVER() as totalCount
                           FROM departments cd
                           WHERE cd.parent_id = @ParentId AND cd.is_active = true
                           ORDER BY created_at
                           LIMIT @ChildSize OFFSET @ChildPage
                           """;

        var dbConnection = _dbConnectionFactory.GetDbConnection();

        long? totalCount = null;

        var childrenDepartment = (await dbConnection.QueryAsync<DepartmentDto, long, DepartmentDto>(
            sql,
            param: new
            {
                query.ParentId,
                ChildPage = (query.Request.Page - 1) * query.Request.PageSize,
                ChildSize = query.Request.PageSize,
            },
            splitOn:"totalCount",
            map: (d, t) =>
            {
                totalCount ??= t;
                return d;
            })).ToList();

        return new PaginationEnvelope<DepartmentDto>(childrenDepartment, totalCount ?? 0);
    }
}