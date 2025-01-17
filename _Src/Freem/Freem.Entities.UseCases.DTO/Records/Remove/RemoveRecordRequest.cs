using Freem.Entities.Records.Identifiers;

namespace Freem.Entities.UseCases.DTO.Records.Remove;

public sealed class RemoveRecordRequest
{
    public RecordIdentifier Id { get; }

    public RemoveRecordRequest(RecordIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}