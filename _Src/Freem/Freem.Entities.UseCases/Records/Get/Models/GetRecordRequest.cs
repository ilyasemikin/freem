using Freem.Entities.Records.Identifiers;

namespace Freem.Entities.UseCases.Records.Get.Models;

public sealed class GetRecordRequest
{
    public RecordIdentifier Id { get; }

    public GetRecordRequest(RecordIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}