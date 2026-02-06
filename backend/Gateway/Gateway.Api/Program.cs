WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

WebApplication app = builder.Build();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("http://localhost:5048/openapi/v1.json", "Directory Service API V1");
    options.SwaggerEndpoint("http://localhost:5048/openapi/v2.json", "Directory Service API V2");

    options.SwaggerEndpoint("http://localhost:5201/openapi/v1.json", "File Service API V1");
});

app.MapReverseProxy();

app.Run();