using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Records.Update;

public sealed class UpdateRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.Update;

    public RecordId Id { get; }
    
    public UpdateField<RecordName?>? Name { get; init; }
    public UpdateField<RecordDescription?>? Description { get; init; }
    
    public UpdateField<RelatedActivityIdsCollection>? Activities { get; init; }
    public UpdateField<RelatedTagIdsCollection>? Tags { get; init; }

    public UpdateRecordRequest(RecordId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }

    public bool HasChanges()
    {
        return 
            Name is not null ||
            Description is not null ||
            Activities is not null ||
            Tags is not null;
    }
}