using System.Globalization;
using DirectoryService.API;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateLogger();

try
{
    Log.Information("Starting directory service");

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    string envName = builder.Environment.EnvironmentName;

    builder.Configuration.AddJsonFile($"appsettings.{envName}.json", true, true);

    builder.Configuration.AddEnvironmentVariables();

    builder.Services.AddConfiguration(builder.Configuration);

    WebApplication app = builder.Build();

    app.Configure();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}