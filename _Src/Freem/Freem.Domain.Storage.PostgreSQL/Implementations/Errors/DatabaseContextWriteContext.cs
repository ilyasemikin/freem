namespace Freem.Domain.Storage.PostgreSQL.Implementations.Errors;

internal sealed class DatabaseContextWriteContext
{
    public Id ProcessedId { get; }

    public DatabaseContextWriteContext(Id processedId)
    {
        ArgumentNullException.ThrowIfNull(processedId);
        
        ProcessedId = processedId;
    }
}