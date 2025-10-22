using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Departments.UseCases.Move;
using DirectoryService.Contracts.Departments.Dtos;
using DirectoryService.Contracts.Departments.Requests;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DirectoryService.IntegrationTests.Departments;

public class MoveDepartmentTests : DirectoryBaseTests
{
    public MoveDepartmentTests(DirectoryTestWebFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task MoveDepartmentToSelfShouldFail()
    {
        // arrange
        var locationId = await CreateLocationAsync("Локация 1");

        var dev = await CreateParentDepartmentAsync("Разработка", "dev", [locationId]);

        var cancellationToken = CancellationToken.None;

        // act
        var result = await ExecuteMoveDepartmentHandler(handler =>
        {
            var command = new MoveDepartmentCommand(
                dev.Id.Value,
                new MoveDepartmentRequest(dev.Id.Value));

            return handler.Handle(command, cancellationToken);
        });

        // assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task MoveDepartmentToChildShouldFail()
    {
        // arrange
        var locationId = await CreateLocationAsync("Локация 1");

        var dev = await CreateParentDepartmentAsync("Разработка", "dev", [locationId]);

        var itOtdel = await CreateChildDepartmentAsync("It отдел", "it-otdel", dev, [locationId]);

        var cancellationToken = CancellationToken.None;

        // act
        var result = await ExecuteMoveDepartmentHandler(handler =>
        {
            var command = new MoveDepartmentCommand(
                dev.Id.Value,
                new MoveDepartmentRequest(itOtdel.Id.Value));

            return handler.Handle(command, cancellationToken);
        });

        // assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task MoveDepartmentWithValidDataShouldSucceed()
    {
        // arrange
        var locationId = await CreateLocationAsync("Локация 1");

        var dev = await CreateParentDepartmentAsync("Разработка", "dev", [locationId]);

        var itOtdel = await CreateChildDepartmentAsync("It отдел", "it-otdel", dev, [locationId]);

        var backend = await CreateChildDepartmentAsync("Бэкендеры", "backend", itOtdel, [locationId]);

        var team1 = await CreateChildDepartmentAsync("Команда 1", "team-one", backend, [locationId]);

        var cancellationToken = CancellationToken.None;

        // act
        var result = await ExecuteMoveDepartmentHandler(handler =>
        {
            var command = new MoveDepartmentCommand(
                backend.Id.Value,
                new MoveDepartmentRequest(dev.Id.Value));

            return handler.Handle(command, cancellationToken);
        });

        // assert
        await ExecuteInDb(async dbContext =>
        {
            const string sql = """
                               SELECT d.* FROM departments d
                               WHERE d.path <@ @departmentPath::ltree
                               ORDER BY d.depth
                               """;

            var dbConnection = dbContext.Database.GetDbConnection();

            var departments = await dbConnection.QueryAsync<DepartmentDto>(
                sql,
                new
                {
                    departmentPath = "dev.backend",
                });

            var backendFromQuery = departments.First(x => x.Id == backend.Id.Value);

            var team1FromQuery = departments.First(x => x.Id == team1.Id.Value);

            Assert.Equal("dev.backend", backendFromQuery.Path);
            Assert.Equal(1, backendFromQuery.Depth);

            Assert.Equal("dev.backend.team-one", team1FromQuery.Path);
            Assert.Equal(2, team1FromQuery.Depth);

            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
        });
    }

    private Task<Result<Guid, Errors>> ExecuteMoveDepartmentHandler(
        Func<MoveDepartmentHandler, Task<Result<Guid, Errors>>> action) =>
        ExecuteHandler(action);
}