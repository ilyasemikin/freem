namespace Freem.Web.Api.Public.Configuration.Instances;

public class ApiConfiguration
{
    public IReadOnlyList<int>? Ports { get; init; }
    public IReadOnlyList<string>? CorsOrigins { get; init; }
}