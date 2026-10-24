using System.Reflection;

namespace Freem.Api.Public.Build;

public sealed class BuildInfo
{
    public required string Version { get; init; }

    public string? CommitHash { get; init; }

    public static BuildInfo FromEntryAssembly()
    {
        var assembly = Assembly.GetEntryAssembly();
        var attribute = assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        var informationalVersion = attribute?.InformationalVersion;

        if (string.IsNullOrEmpty(informationalVersion))
        {
            return new BuildInfo { Version = "unknown" };
        }

        var separatorIndex = informationalVersion.IndexOf('+');
        if (separatorIndex < 0)
        {
            return new BuildInfo { Version = informationalVersion };
        }

        var version = informationalVersion[..separatorIndex];
        var commitHash = informationalVersion[(separatorIndex + 1)..];
        return new BuildInfo { Version = version, CommitHash = commitHash };
    }
}
