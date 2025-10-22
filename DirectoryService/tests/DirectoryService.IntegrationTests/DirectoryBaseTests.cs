using CSharpFunctionalExtensions;
using DirectoryService.Domain.DepartmentLocations;
using DirectoryService.Domain.DepartmentLocations.ValueObjects;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Departments.ValueObjects;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Locations.ValueObjects;
using DirectoryService.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace DirectoryService.IntegrationTests;

public class DirectoryBaseTests : IClassFixture<DirectoryTestWebFactory>, IAsyncLifetime
{
    private readonly IServiceProvider _serviceProvider;

    private readonly Func<Task> _resetDatabaseAsync;

    public DirectoryBaseTests(DirectoryTestWebFactory factory)
    {
        _serviceProvider = factory.Services;
        _resetDatabaseAsync = factory.ResetDatabaseAsync;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _resetDatabaseAsync();
    }

    protected async Task<LocationId> CreateLocationAsync(string name)
    {
        var locationId = await ExecuteInDb(async dbContext =>
        {
            var random = new Random();

            string randomString = random.Next(100000, 999999).ToString();

            var location = new Location(
                LocationId.Create(),
                LocationName.Of(name).Value,
                Timezone.Of("Europe/Moscow").Value,
                Address.Of("123", randomString, "123", "123", "123", "1").Value);

            dbContext.Locations.Add(location);
            await dbContext.SaveChangesAsync();

            return location.Id;
        });

        return locationId;
    }

    protected async Task<Department> CreateParentDepartmentAsync(string name, string identifier, IEnumerable<LocationId> locationIds)
    {
        var departmentId = DepartmentId.Create();

        var departmentLocationIds = locationIds
            .Select(locationId =>
                new DepartmentLocation(DepartmentLocationId.Create(), departmentId, locationId));

        var parent = Department.CreateParent(
            DepartmentName.Of(name).Value,
            Identifier.Of(identifier).Value,
            departmentLocationIds,
            departmentId).Value;

        await ExecuteInDb(async dbContext =>
        {
            dbContext.Departments.Add(parent);
            await dbContext.SaveChangesAsync();
        });

        return parent;
    }

    protected async Task<Department> CreateChildDepartmentAsync(string name, string identifier, Department parent, IEnumerable<LocationId> locationIds)
    {
        var departmentId = DepartmentId.Create();

        var departmentLocationIds = locationIds
            .Select(locationId =>
                new DepartmentLocation(DepartmentLocationId.Create(), departmentId, locationId));

        var child = Department.CreateChild(
            DepartmentName.Of(name).Value,
            Identifier.Of(identifier).Value,
            parent,
            departmentLocationIds,
            departmentId).Value;

        await ExecuteInDb(async dbContext =>
        {
            dbContext.Departments.Add(child);
            await dbContext.SaveChangesAsync();
        });

        return child;
    }

    protected async Task<TResult> ExecuteHandler<TResult, THandler>(Func<THandler, Task<TResult>> action)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var handler = scope.ServiceProvider.GetRequiredService<THandler>();
        return await action(handler);
    }

    protected async Task<T> ExecuteInDb<T>(Func<AppDbContext, Task<T>> action)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        return await action(dbContext);
    }

    protected async Task ExecuteInDb(Func<AppDbContext, Task> action)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await action(dbContext);
    }
}