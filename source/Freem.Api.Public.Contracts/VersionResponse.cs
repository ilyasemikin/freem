namespace Freem.Api.Public.Contracts;

/// <summary>Build information of the running service.</summary>
public sealed class VersionResponse
{
    /// <summary>Service version.</summary>
    public required string Version { get; init; }

    /// <summary>Commit the build was made from, if known.</summary>
    public string? CommitHash { get; init; }
}
