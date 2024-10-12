using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Records.Models;
using Freem.Entities.UseCases.Models.Fields;

namespace Freem.Entities.UseCases.Records.Update.Models;

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
}