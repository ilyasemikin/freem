using Freem.Domain.Records;

namespace Freem.Api.Public.Contracts.DTO.Records;

/// <summary>Result of creating a record.</summary>
public sealed class CreateRecordResponse
{
    /// <summary>Id of the created record.</summary>
    public RecordId Id { get; }
    
    public CreateRecordResponse(RecordId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}