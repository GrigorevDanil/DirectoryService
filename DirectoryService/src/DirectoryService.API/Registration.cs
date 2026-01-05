using DirectoryService.Application;
using DirectoryService.Infrastructure;
using Serilog;
using SharedService.Framework.Endpoints;
using SharedService.Framework.Logging;
using SharedService.Framework.Middlewares;
using SharedService.Framework.Swagger;

namespace DirectoryService.API;

public static class Registration
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors();

        services.AddControllers();

        services
            .AddApplication()
            .AddInfrastructure(configuration);

        services
            .AddSerilogLogging(configuration, "FileService")
            .AddCustomSwagger(configuration)
            .AddEndpoints(typeof(Registration).Assembly);

        return services;
    }

    public static IApplicationBuilder Configure(this WebApplication app)
    {
        app.UseCors(builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });

        app.UseExceptionMiddleware();
        app.UseRequestCorrelationId();
        app.UseSerilogRequestLogging();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();

        return app;
    }
}