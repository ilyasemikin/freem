using Microsoft.Extensions.Logging;

namespace Freem.Domain.Storage.PostgreSQL.DependencyInjection;

public sealed class StorageOptions
{
    public delegate void LoggerAction(string message);

    public LoggerAction? Logger { get; init; }
    public ILoggerFactory? LoggerFactory { get; init; }

    public bool EnableServiceProviderCaching { get; init; } = true;
}
