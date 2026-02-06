using Asp.Versioning;
using DirectoryService.Application;
using DirectoryService.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi;
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
        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.AssumeDefaultVersionWhenUnspecified = true;

                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("x-api-version"));

                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        services.AddCors();

        services.AddControllers();

        services
            .AddApplication(configuration)
            .AddInfrastructure(configuration);

        services
            .AddSerilogLogging(configuration, "DirectoryService")
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