namespace Freem.Api.Public.Configuration.Subsystems;

internal sealed class IntegrationsConfigurationProperties
{
    public required EgoHubClientConfigurationProperties EgoHubClient { get; init; }

    /// <summary>Service API key issued by EgoHub (Admin API), in "&lt;keyId&gt;:&lt;secret&gt;" form.</summary>
    public required string EgoHubApiKey { get; init; }
}
