using CSharpFunctionalExtensions;
using DirectoryService.Application.SoftDelete;
using Microsoft.EntityFrameworkCore;
using SharedService.SharedKernel;

namespace DirectoryService.IntegrationTests.SoftDelete;

public class DeletedRecordsCleanerTests : DirectoryBaseTests
{
    public DeletedRecordsCleanerTests(DirectoryTestWebFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task DeleteChildDepartmentShouldTrue()
    {
        // arrange
        var locationId1 = await CreateLocationAsync("Локация 1");

        var locationId2 = await CreateDeletedLocationAsync("Локация 2");

        var dev = await CreateParentDepartmentAsync("Разработка", "dev", [locationId1]);

        var itOtdel = await CreateDeletedChildDepartmentAsync("It отдел", "it-otdel", dev, [locationId2]);

        var backend = await CreateChildDepartmentAsync("Бэкендеры", "backend", itOtdel, [locationId1]);

        await CreateChildDepartmentAsync("Команда 1", "team-one", backend, [locationId1]);

        var cancellationToken = CancellationToken.None;

        // act
        var result = await ExecuteDeletedRecordsClean(handler =>
            handler.Process(cancellationToken));

        // assert
        await ExecuteInDb(async dbContext =>
        {
            var dbDev = await dbContext.Departments.FirstOrDefaultAsync(x => x.Id == dev.Id, cancellationToken);

            var movedDepartment = await dbContext.Departments.FirstOrDefaultAsync(x => x.Id == backend.Id, cancellationToken);

            var deletedLocation = await dbContext.Locations.FirstOrDefaultAsync(x => x.Id == locationId2, cancellationToken);

            var deletedDepartment = await dbContext.Departments.FirstOrDefaultAsync(x => x.Id == itOtdel.Id, cancellationToken);

            Assert.Null(deletedLocation);

            Assert.Null(deletedDepartment);

            Assert.Equal(dbDev!.Id, movedDepartment!.ParentId);
            Assert.Equal("dev.backend", movedDepartment!.Path.Value);

            Assert.True(result.IsSuccess);
        });
    }

    [Fact]
    public async Task DeleteRootDepartmentShouldTrue()
    {
        // arrange
        var locationId1 = await CreateLocationAsync("Локация 1");

        var locationId2 = await CreateDeletedLocationAsync("Локация 2");

        var dev = await CreateDeletedParentDepartmentAsync("Разработка", "dev", [locationId1]);

        var itOtdel = await CreateChildDepartmentAsync("It отдел", "it-otdel", dev, [locationId2]);

        var backend = await CreateChildDepartmentAsync("Бэкендеры", "backend", itOtdel, [locationId1]);

        await CreateChildDepartmentAsync("Команда 1", "team-one", backend, [locationId1]);

        var cancellationToken = CancellationToken.None;

        // act
        var result = await ExecuteDeletedRecordsClean(handler =>
            handler.Process(cancellationToken));

        // assert
        await ExecuteInDb(async dbContext =>
        {
            var dbItOtdel = await dbContext.Departments.FirstOrDefaultAsync(x => x.Id == itOtdel.Id, cancellationToken);

            var movedDepartment = await dbContext.Departments.FirstOrDefaultAsync(x => x.Id == backend.Id, cancellationToken);

            var deletedLocation = await dbContext.Locations.FirstOrDefaultAsync(x => x.Id == locationId2, cancellationToken);

            var deletedDepartment = await dbContext.Departments.FirstOrDefaultAsync(x => x.Id == dev.Id, cancellationToken);

            Assert.Null(deletedLocation);

            Assert.Null(deletedDepartment);

            Assert.Equal("it-otdel.backend", movedDepartment!.Path.Value);

            Assert.Null(dbItOtdel!.ParentId);

            Assert.True(result.IsSuccess);
        });
    }

    private Task<UnitResult<Error>> ExecuteDeletedRecordsClean(
        Func<IDeletedRecordsCleanerService, Task<UnitResult<Error>>> action) =>
        Execute(action);
}