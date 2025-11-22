using Dapper;
using DirectoryService.Application.Constants;
using DirectoryService.Contracts.Departments.Dtos;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Abstractions;
using Shared.Caching;
using Shared.Database;

namespace DirectoryService.Application.Departments.Queries.GetTopFiveDepartmentsWithMostPositions;

public class GetTopFiveDepartmentsWithMostPositionsHandler : IQueryHandler<DepartmentDto[]>
{
    private readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
    };

    private readonly IDbConnectionFactory _dbConnectionFactory;

    private readonly ICacheService _cache;

    public GetTopFiveDepartmentsWithMostPositionsHandler(IDbConnectionFactory dbConnectionFactory, ICacheService cache)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _cache = cache;
    }

    public async Task<DepartmentDto[]> Handle(CancellationToken cancellationToken = default)
    {
        return await _cache.GetOrSetAsync(CachingKeys.CreateDepartmentsKey("top"), async () =>
        {
            const string sql = """
                               SELECT d.* FROM departments d
                               LEFT JOIN department_positions dp ON dp.department_id = d.id
                               GROUP BY d.id
                               ORDER BY COUNT(dp.id) DESC 
                               LIMIT 5
                               """;

            var dbConnection = _dbConnectionFactory.GetDbConnection();

            var res = await dbConnection.QueryAsync<DepartmentDto>(sql, cancellationToken);

            return res.ToArray();
        }, _cacheOptions, cancellationToken) ?? [];
    }
}