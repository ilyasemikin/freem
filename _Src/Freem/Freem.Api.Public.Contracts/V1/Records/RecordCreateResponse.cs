using Freem.Domain.Records;

namespace Freem.Api.Public.Contracts.V1.Records;

/// <summary>Result of creating a record.</summary>
public sealed class RecordCreateResponse
{
    /// <summary>Id of the created record.</summary>
    public RecordId Id { get; }
    
    public RecordCreateResponse(RecordId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}