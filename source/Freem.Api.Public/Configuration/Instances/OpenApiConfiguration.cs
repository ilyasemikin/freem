namespace Freem.Api.Public.Configuration.Instances;

public sealed class OpenApiConfiguration
{
    public required IReadOnlyList<string> ServerAddresses { get; init; }
}