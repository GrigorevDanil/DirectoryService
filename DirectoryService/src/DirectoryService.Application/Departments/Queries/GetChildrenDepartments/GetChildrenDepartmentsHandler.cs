using Dapper;
using DirectoryService.Contracts.Departments.Dtos;
using Shared;
using Shared.Abstractions;
using Shared.Database;

namespace DirectoryService.Application.Departments.Queries.GetChildrenDepartments;

public class GetChildrenDepartmentsHandler : IQueryHandler<GetChildrenDepartmentsQuery, PaginationEnvelope<DepartmentDto>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetChildrenDepartmentsHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<PaginationEnvelope<DepartmentDto>> Handle(
        GetChildrenDepartmentsQuery query,
        CancellationToken cancellationToken = default)
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