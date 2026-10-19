using Freem.Domain.Records;

namespace Freem.Web.Api.Public.Contracts.DTO.Records;

public sealed class CreateRecordResponse
{
    public RecordId Id { get; }
    
    public CreateRecordResponse(RecordId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}