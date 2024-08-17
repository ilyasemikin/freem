namespace Freem.Entities.Storage.PostgreSQL.DependencyInjection;

public sealed class StorageConfiguration
{
    public delegate void LoggerAction(string message);
    
    public string ConnectionString { get; }
    
    public LoggerAction? Logger { get; init; }
    
    public StorageConfiguration(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        
        ConnectionString = connectionString;
    }
}