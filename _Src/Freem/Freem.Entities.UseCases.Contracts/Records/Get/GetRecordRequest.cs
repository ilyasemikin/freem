using Freem.Entities.Records.Identifiers;

namespace Freem.Entities.UseCases.Contracts.Records.Get;

public sealed class GetRecordRequest
{
    public RecordIdentifier Id { get; }

    public GetRecordRequest(RecordIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}