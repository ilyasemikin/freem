using Freem.Entities.Records.Identifiers;

namespace Freem.Entities.UseCases.Records.Remove.Models;

public sealed class RemoveRecordRequest
{
    public RecordIdentifier Id { get; }

    public RemoveRecordRequest(RecordIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}