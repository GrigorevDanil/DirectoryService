using DirectoryService.Application;
using DirectoryService.Infrastructure;
using Serilog;
using SharedService.Framework.Endpoints;
using SharedService.Framework.Logging;
using SharedService.Framework.Middlewares;
using SharedService.Framework.Swagger;
using SharedService.Framework.Versioning;

namespace DirectoryService.API;

public static class Registration
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddVersioning();

        services.AddCors();

        services.AddControllers();

        services
            .AddApplication(configuration)
            .AddInfrastructure(configuration);

        services
            .AddSerilogLogging(configuration, "DirectoryService")
            .AddEndpoints(typeof(Registration).Assembly);

        services.AddCustomOpenApi(configuration);

        return services;
    }

    public static IApplicationBuilder Configure(this WebApplication app)
    {
        app.UseCors(builder =>
        {
            builder.WithOrigins("http://localhost:3000", "http://localhost")
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });

        app.UseExceptionMiddleware();
        app.UseRequestCorrelationId();
        app.UseSerilogRequestLogging();

        app.MapOpenApi();

        app.UseCustomSwaggerUI();

        app.MapControllers();

        return app;
    }
}