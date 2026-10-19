using Freem.Domain.Records;

namespace Freem.Entities.UseCases.Contracts.Records.Remove;

public sealed class RemoveRecordRequest
{
    public RecordId Id { get; }

    public RemoveRecordRequest(RecordId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}