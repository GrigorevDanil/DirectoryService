using System.Data.Common;
using Dapper;
using DirectoryService.Contracts.Departments.Dtos;
using DirectoryService.Contracts.Locations.Dtos;
using DirectoryService.Contracts.Positions.Dtos;
using SharedService.Core.Database;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Departments.Queries.GetDetail;

public class GetDetailDepartmentHandler : IQueryHandler<GetDetailDepartmentQuery, DepartmentDetailDto?>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetDetailDepartmentHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<DepartmentDetailDto?> Handle(
        GetDetailDepartmentQuery query,
        CancellationToken cancellationToken = new CancellationToken())
    {
        string sql = $"""
                      SELECT d.*, l.*, p.*
                      FROM departments d
                      LEFT JOIN department_locations dl ON  dl.department_id = d.id
                      LEFT JOIN locations l ON  l.id = dl.location_id
                      LEFT JOIN department_positions dp ON  dp.department_id = d.id
                      LEFT JOIN positions p ON  p.id = dp.position_id
                      WHERE d.id = '{query.Id}'
                      """;

        DbConnection dbConnection = _dbConnectionFactory.GetDbConnection();

        List<LocationDto> locationDtos = [];

        List<PositionDto> positionDtos = [];

        IEnumerable<DepartmentDetailDto> res = await dbConnection.QueryAsync<DepartmentDetailDto, LocationDto, PositionDto, DepartmentDetailDto>(
            sql,
            splitOn: "id,id",
            map: (d, l, p) =>
            {
                locationDtos.Add(l);
                positionDtos.Add(p);
                return d;
            });

        DepartmentDetailDto? departmentDetailDto = res.FirstOrDefault();

        departmentDetailDto?.Locations.AddRange(locationDtos);

        departmentDetailDto?.Positions.AddRange(positionDtos);

        return departmentDetailDto;
    }
}