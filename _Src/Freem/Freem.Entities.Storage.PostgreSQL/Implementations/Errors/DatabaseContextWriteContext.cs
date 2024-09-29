using Freem.Entities.Abstractions.Identifiers;
using Freem.Identifiers.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors;

internal sealed class DatabaseContextWriteContext
{
    public IIdentifier ProcessedId { get; }

    public DatabaseContextWriteContext(IIdentifier processedId)
    {
        ArgumentNullException.ThrowIfNull(processedId);
        
        ProcessedId = processedId;
    }
}