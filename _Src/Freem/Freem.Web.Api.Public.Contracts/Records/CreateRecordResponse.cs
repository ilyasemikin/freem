using Freem.Entities.Identifiers;

namespace Freem.Web.Api.Public.Contracts.Records;

public sealed class CreateRecordResponse
{
    public RecordIdentifier Id { get; }
    
    public CreateRecordResponse(RecordIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}