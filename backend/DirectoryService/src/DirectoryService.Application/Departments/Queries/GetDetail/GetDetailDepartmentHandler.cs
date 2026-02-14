using System.Data.Common;
using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Contracts.Departments.Dtos;
using DirectoryService.Contracts.Locations.Dtos;
using DirectoryService.Contracts.Positions.Dtos;
using FileService.Contracts.HttpCommunication;
using FileService.Contracts.MediaAssets.Dtos;
using SharedService.Core.Database;
using SharedService.Core.Handlers;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Departments.Queries.GetDetail;

public class GetDetailDepartmentHandler : IQueryHandlerWithResult<GetDetailDepartmentQuery, DepartmentDetailDto?>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IFileCommunicationService _fileCommunicationService;

    public GetDetailDepartmentHandler(IDbConnectionFactory dbConnectionFactory, IFileCommunicationService fileCommunicationService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _fileCommunicationService = fileCommunicationService;
    }

    public async Task<Result<DepartmentDetailDto?, Errors>> Handle(
        GetDetailDepartmentQuery query,
        CancellationToken cancellationToken = new CancellationToken())
    {
        string sql = $"""
                      SELECT d.*, d.video_id, l.*, p.*
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

        Guid videoId = Guid.Empty;

        IEnumerable<DepartmentDetailDto> res = await dbConnection.QueryAsync<DepartmentDetailDto, Guid?, LocationDto, PositionDto, DepartmentDetailDto>(
            sql,
            splitOn: "video_id,id,id",
            map: (d, vId, l, p) =>
            {
                locationDtos.Add(l);
                positionDtos.Add(p);
                videoId = vId ?? Guid.Empty;
                return d;
            });

        DepartmentDetailDto? departmentDetailDto = res.FirstOrDefault();

        if (videoId != Guid.Empty)
        {
            Result<MediaAssetDto?, Errors> getMediaResult = await _fileCommunicationService.GetMediaAsset(videoId, cancellationToken);

            if (getMediaResult.IsFailure)
                return getMediaResult.Error;

            MediaAssetDto? mediaAsset = getMediaResult.Value;

            departmentDetailDto?.VideoUrl = mediaAsset?.DownloadUrl;
        }

        departmentDetailDto?.Locations.AddRange(locationDtos);

        departmentDetailDto?.Positions.AddRange(positionDtos);

        return departmentDetailDto;
    }
}