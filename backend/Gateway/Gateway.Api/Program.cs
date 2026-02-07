using System.Globalization;
using Gateway.Api;
using Microsoft.Extensions.Options;
using Serilog;
using SharedService.Framework.Logging;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateLogger();

try
{
    Log.Information("Starting gateway");

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    string envName = builder.Environment.EnvironmentName;

    builder.Configuration.AddJsonFile($"appsettings.{envName}.json", true, true);

    builder.Configuration.AddEnvironmentVariables();

    builder.Services.AddSerilogLogging(builder.Configuration, "Gateway");

    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

    builder.Services.Configure<SwaggerConfig>(
        builder.Configuration.GetSection("SwaggerConfig"));

    WebApplication app = builder.Build();

    app.UseSwaggerUI(options =>
    {
        SwaggerConfig config = app.Services.GetRequiredService<IOptions<SwaggerConfig>>().Value;

        foreach (SwaggerEndpoint endpoint in config.Endpoints)
        {
            options.SwaggerEndpoint(endpoint.Url, endpoint.Name);
        }
    });

    app.MapReverseProxy();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}