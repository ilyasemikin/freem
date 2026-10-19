using Freem.Domain;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors;

internal sealed class DatabaseContextWriteContext
{
    public Id ProcessedId { get; }

    public DatabaseContextWriteContext(Id processedId)
    {
        ArgumentNullException.ThrowIfNull(processedId);
        
        ProcessedId = processedId;
    }
}