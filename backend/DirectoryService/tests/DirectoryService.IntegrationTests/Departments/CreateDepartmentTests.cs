using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments.UseCases.Create;
using DirectoryService.Contracts.Departments.Requests;
using DirectoryService.Domain.Departments.ValueObjects;
using Microsoft.EntityFrameworkCore;
using SharedService.SharedKernel;

namespace DirectoryService.IntegrationTests.Departments;

public class CreateDepartmentTests : DirectoryBaseTests
{
    public CreateDepartmentTests(DirectoryTestWebFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task CreateDepartmentWithDuplicateShouldFail()
    {
        // arrange
        var locationId = await CreateLocationAsync("Локация 1");

        await CreateParentDepartmentAsync("Подразделение", "dep", [locationId]);

        var cancellationToken = CancellationToken.None;

        // act
        var result = await ExecuteCreateDepartmentHandler(handler =>
        {
            var command = new CreateDepartmentCommand(
                new CreateDepartmentRequest(
                    "Подразделение",
                    "dep",
                    null,
                    [locationId.Value]));

            return handler.Handle(command, cancellationToken);
        });

        // assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task CreateDepartmentWithoutLocationIdsShouldFail()
    {
        // arrange
        var cancellationToken = CancellationToken.None;

        // act
        var result = await ExecuteCreateDepartmentHandler(handler =>
        {
            var command = new CreateDepartmentCommand(
                new CreateDepartmentRequest(
                    "Подразделение",
                    "dep",
                    null,
                    []));

            return handler.Handle(command, cancellationToken);
        });

        // assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task CreateDepartmentWithoutParentShouldSucceed()
    {
        // arrange
        var locationId = await CreateLocationAsync("Локация 1");

        var cancellationToken = CancellationToken.None;

        // act
        var result = await ExecuteCreateDepartmentHandler(handler =>
        {
            var command = new CreateDepartmentCommand(
                new CreateDepartmentRequest(
                    "Подразделение",
                    "dep",
                    null,
                    [locationId.Value]));

            return handler.Handle(command, cancellationToken);
        });

        // assert
        await ExecuteInDb(async dbContext =>
        {
            var department = await dbContext.Departments.FirstAsync(x => x.Id == DepartmentId.Of(result.Value), cancellationToken);

            Assert.NotNull(department);
            Assert.Equal(department.Id.Value, result.Value);

            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
        });
    }

    [Fact]
    public async Task CreateDepartmentWithParentShouldSucceed()
    {
        // arrange
        var locationId = await CreateLocationAsync("Локация 1");

        var parent = await CreateParentDepartmentAsync("Подразделение", "dep", [locationId]);

        var cancellationToken = CancellationToken.None;

        // act
        var result = await ExecuteCreateDepartmentHandler(handler =>
        {
            var command = new CreateDepartmentCommand(
                new CreateDepartmentRequest(
                    "Подразделение 1",
                    "dep-child",
                    parent.Id.Value,
                    [locationId.Value]));

            return handler.Handle(command, cancellationToken);
        });

        // assert
        await ExecuteInDb(async dbContext =>
        {
            var department = await dbContext.Departments.FirstAsync(x => x.Id == DepartmentId.Of(result.Value), cancellationToken);

            Assert.NotNull(department);
            Assert.Equal(department.Id.Value, result.Value);

            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
        });
    }

    private Task<Result<Guid, Errors>> ExecuteCreateDepartmentHandler(
        Func<CreateDepartmentHandler, Task<Result<Guid, Errors>>> action) =>
        Execute(action);
}