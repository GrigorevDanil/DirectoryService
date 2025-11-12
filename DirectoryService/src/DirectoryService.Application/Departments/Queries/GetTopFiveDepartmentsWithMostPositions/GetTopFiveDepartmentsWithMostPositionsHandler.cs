using Dapper;
using DirectoryService.Contracts.Departments.Dtos;
using Shared.Abstractions;
using Shared.Database;

namespace DirectoryService.Application.Departments.Queries.GetTopFiveDepartmentsWithMostPositions;

public class GetTopFiveDepartmentsWithMostPositionsHandler : IQueryHandler<DepartmentDto[]>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetTopFiveDepartmentsWithMostPositionsHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<DepartmentDto[]> Handle(CancellationToken cancellationToken = default)
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
    }
}