using System.Reflection;
using Dapper;
using DirectoryService.Application.Database;
using DirectoryService.Application.Departments;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Positions;
using DirectoryService.Infrastructure.Database;
using DirectoryService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Dapper;
using Shared.Database;

namespace DirectoryService.Infrastructure;

public static class Registration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return
            services
                .AddRepositories()
                .AddDatabase(configuration);
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ILocationRepository, LocationsRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentsRepository>();
        services.AddScoped<IPositionsRepository, PositionsRepository>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AppDbContext>(_ =>
            new AppDbContext(configuration.GetConnectionString("DirectoryServiceDb")!));
        services.AddScoped<IReadDbContext, AppDbContext>(_ =>
            new AppDbContext(configuration.GetConnectionString("DirectoryServiceDb")!));
        services.AddScoped<ITransactionManager, TransactionManager>();

        services.AddDapper(configuration);

        return services;
    }

    private static IServiceCollection AddDapper(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbConnectionFactory, AppDbContext>(_ =>
            new AppDbContext(configuration.GetConnectionString("DirectoryServiceDb")!));

        DefaultTypeMap.MatchNamesWithUnderscores = true;

        var assembly = Assembly.GetAssembly(typeof(Contracts.Registration));

        if (assembly != null)
        {
            var jsonTypes = assembly.GetTypes()
                .Where(t => t.IsClass && typeof(IDapperJson).IsAssignableFrom(t))
                .ToList();

            foreach (var type in jsonTypes)
            {
                var handlerType = typeof(JsonTypeHandler<>).MakeGenericType(type);
                object? handler = Activator.CreateInstance(handlerType);
                SqlMapper.AddTypeHandler(type, handler as SqlMapper.ITypeHandler ?? throw new InvalidOperationException());
            }
        }

        return services;
    }
}