using DirectoryService.Application.Abstractions.Locations;
using DirectoryService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Application;

public static class Registration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return
            services.AddServices();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services.AddScoped<ILocationsService, LocationsService>();
    }
}