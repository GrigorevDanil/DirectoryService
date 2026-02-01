using System.Data.Common;
using Dapper;
using DirectoryService.Contracts.Departments.Dtos;
using DirectoryService.Contracts.Positions.Dtos;
using SharedService.Core.Database;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Positions.Queries.GetDetail;

public class GetDetailPositionHandler : IQueryHandler<GetDetailPositionQuery, PositionDetailDto?>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetDetailPositionHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<PositionDetailDto?> Handle(
        GetDetailPositionQuery query,
        CancellationToken cancellationToken = new CancellationToken())
    {
        string sql = $"""
                     SELECT p.*, d.id, d.name, d.identifier, d.path, d.is_active, d.created_at, d.updated_at, d.deleted_at
                     FROM positions p
                     JOIN department_positions dp ON  dp.position_id = p.id
                     JOIN departments d ON  d.id = dp.department_id
                     WHERE p.id = '{query.Id}'
                     """;

        DbConnection dbConnection = _dbConnectionFactory.GetDbConnection();

        List<DepartmentShortDto> departmentDtos = [];

        IEnumerable<PositionDetailDto> res = await dbConnection.QueryAsync<PositionDetailDto, DepartmentShortDto, PositionDetailDto>(
            sql,
            splitOn: "id",
            map: (p, dp) =>
            {
                departmentDtos.Add(dp);
                return p;
            });

        PositionDetailDto? positionDetailDto = res.FirstOrDefault();

        positionDetailDto?.Departments.AddRange(departmentDtos);

        return positionDetailDto;
    }
}