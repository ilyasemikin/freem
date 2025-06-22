using Freem.Entities.Records.Identifiers;

namespace Freem.Web.Api.Public.Contracts.DTO.Records;

public sealed class CreateRecordResponse
{
    public RecordIdentifier Id { get; }
    
    public CreateRecordResponse(RecordIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}