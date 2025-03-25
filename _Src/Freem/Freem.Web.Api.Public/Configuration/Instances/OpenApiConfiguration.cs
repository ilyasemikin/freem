namespace Freem.Web.Api.Public.Configuration.Instances;

public sealed class OpenApiConfiguration
{
    public required IReadOnlyList<string> ServerAddresses { get; init; }
}