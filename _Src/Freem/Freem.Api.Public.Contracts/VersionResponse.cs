namespace Freem.Api.Public.Contracts;

public sealed class VersionResponse
{
    public required string Version { get; init; }

    public string? CommitHash { get; init; }
}
