using System.Reflection;
using Dapper;
using DirectoryService.Application.Database;
using DirectoryService.Application.Departments;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Positions;
using DirectoryService.Application.SoftDelete;
using DirectoryService.Infrastructure.Database;
using DirectoryService.Infrastructure.Repositories;
using DirectoryService.Infrastructure.SoftDelete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedService.Core.Caching;
using SharedService.Core.Database;

namespace DirectoryService.Infrastructure;

public static class Registration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return
            services
                .AddRepositories()
                .AddDistributedCache(configuration)
                .AddSoftDelete(configuration)
                .AddDatabase(configuration);
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ILocationRepository, LocationsRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentsRepository>();
        services.AddScoped<IPositionsRepository, PositionsRepository>();

        return services;
    }

    private static IServiceCollection AddSoftDelete(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDeletedRecordsCleanerService, DeletedRecordsCleanerService>();
        services.AddHostedService<DeletedRecordsCleanerBackgroundService>();
        services.Configure<SoftDeleteSettings>(
            configuration.GetSection("SoftDeleteSettings"));

        return services;
    }

    private static IServiceCollection AddDistributedCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis") ??
                                    throw new ArgumentNullException(nameof(configuration));
        });

        services.AddSingleton<ICacheService, DistributedCacheService>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AppDbContext>(_ =>
            new AppDbContext(configuration.GetConnectionString("DirectoryServiceDb")!));
        services.AddScoped<IReadDbContext, AppDbContext>(_ =>
            new AppDbContext(configuration.GetConnectionString("DirectoryServiceDb")!));
        services.AddScoped<ITransactionManager, TransactionManager>();

        services.AddScoped<IDbConnectionFactory, AppDbContext>(_ =>
            new AppDbContext(configuration.GetConnectionString("DirectoryServiceDb")!));

        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddDapperJson(Assembly.Load("DirectoryService.Contracts"));

        return services;
    }
}