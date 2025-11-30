using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments.UseCases.SetLocations;
using DirectoryService.Contracts.Departments.Requests;
using DirectoryService.Domain.Departments.ValueObjects;
using Microsoft.EntityFrameworkCore;
using SharedService.SharedKernel;

namespace DirectoryService.IntegrationTests.Departments;

public class SetLocationsForDepartmentTests : DirectoryBaseTests
{
    public SetLocationsForDepartmentTests(DirectoryTestWebFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task SetLocationsWithoutLocationIdsShouldFail()
    {
        // arrange
        var departmentId = DepartmentId.Create();

        var locationId = await CreateLocationAsync("Локация");

        await CreateParentDepartmentAsync("Подразделение", "dep", [locationId]);

        var cancellationToken = CancellationToken.None;

        // act
        var result = await ExecuteSetLocationsForDepartmentHandler(handler =>
        {
            var command = new SetLocationsForDepartmentCommand(
                departmentId.Value,
                new SetLocationsForDepartmentRequest([]));

            return handler.Handle(command, cancellationToken);
        });

        // assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task SetLocationsWithValidDataShouldSucceed()
    {
        // arrange
        var locationId1 = await CreateLocationAsync("Локация 1");

        var locationId2 = await CreateLocationAsync("Локация 2");

        var parent = await CreateParentDepartmentAsync("Подразделение", "dep", [locationId1]);

        var cancellationToken = CancellationToken.None;

        // act
        var result = await ExecuteSetLocationsForDepartmentHandler(handler =>
        {
            var command = new SetLocationsForDepartmentCommand(
                parent.Id.Value,
                new SetLocationsForDepartmentRequest([locationId2.Value]));

            return handler.Handle(command, cancellationToken);
        });

        // assert
        await ExecuteInDb(async dbContext =>
        {
            var existedDepartmentLocation = await dbContext.DepartmentLocations
                .FirstAsync(x => x.LocationId == locationId2, cancellationToken);

            var notExistedDepartmentLocation = await dbContext.DepartmentLocations
                .FirstOrDefaultAsync(x => x.LocationId == locationId1, cancellationToken);

            Assert.NotNull(existedDepartmentLocation);
            Assert.Null(notExistedDepartmentLocation);
            Assert.True(result.IsSuccess);
        });
    }

    private Task<Result<Guid, Errors>> ExecuteSetLocationsForDepartmentHandler(
        Func<SetLocationsForDepartmentHandler, Task<Result<Guid, Errors>>> action) =>
        Execute(action);
}