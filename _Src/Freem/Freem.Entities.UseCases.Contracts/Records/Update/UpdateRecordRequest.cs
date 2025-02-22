using Freem.Entities.Identifiers;
using Freem.Entities.Models.Records;
using Freem.Entities.Relations.Collections;

namespace Freem.Entities.UseCases.Contracts.Records.Update;

public sealed class UpdateRecordRequest
{
    public RecordIdentifier Id { get; }
    
    public UpdateField<RecordName>? Name { get; init; }
    public UpdateField<RecordDescription>? Description { get; init; }
    
    public UpdateField<RelatedActivitiesCollection>? Activities { get; init; }
    public UpdateField<RelatedTagsCollection>? Tags { get; init; }

    public UpdateRecordRequest(RecordIdentifier id)
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