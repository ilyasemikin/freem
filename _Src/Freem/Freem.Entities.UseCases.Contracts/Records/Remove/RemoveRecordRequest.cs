using Freem.Entities.Identifiers;

namespace Freem.Entities.UseCases.Contracts.Records.Remove;

public sealed class RemoveRecordRequest
{
    public RecordIdentifier Id { get; }

    public RemoveRecordRequest(RecordIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}