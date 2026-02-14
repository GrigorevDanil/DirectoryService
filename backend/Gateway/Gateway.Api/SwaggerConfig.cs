namespace Gateway.Api;

public class SwaggerConfig
{
    public IReadOnlyList<SwaggerEndpoint> Endpoints { get; init; } = [];
}

public class SwaggerEndpoint
{
    public string Url { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}