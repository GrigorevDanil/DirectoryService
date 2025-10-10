using DirectoryService.Application.Locations;
using DirectoryService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Infrastructure;

public static class Registration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return
            services.AddScoped<AppDbContext>(_ => new AppDbContext(configuration.GetConnectionString("DirectoryServiceDb")!))
                .AddRepositories();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped<ILocationRepository, LocationRepository>();
    }
}