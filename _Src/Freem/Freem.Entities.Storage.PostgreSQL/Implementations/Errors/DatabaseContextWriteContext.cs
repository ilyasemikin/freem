using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors;

internal sealed class DatabaseContextWriteContext
{
    public IEntityIdentifier ProcessedEntityId { get; }

    public DatabaseContextWriteContext(IEntityIdentifier processedEntityId)
    {
        ArgumentNullException.ThrowIfNull(processedEntityId);
        
        ProcessedEntityId = processedEntityId;
    }
}