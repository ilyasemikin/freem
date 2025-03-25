namespace Freem.Web.Api.Public.Configuration.Instances;

public sealed class ApiInfoConfiguration
{
    public required IReadOnlyList<string> ServerAddresses { get; init; }
}