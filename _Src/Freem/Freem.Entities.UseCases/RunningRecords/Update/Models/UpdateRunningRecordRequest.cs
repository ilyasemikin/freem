using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Records.Models;
using Freem.Entities.UseCases.Models.Fields;

namespace Freem.Entities.UseCases.RunningRecords.Update.Models;

public sealed class UpdateRunningRecordRequest
{
    public UpdateField<RecordName>? Name { get; init; }
    public UpdateField<RecordDescription>? Description { get; init; }
    
    public UpdateField<RelatedActivitiesCollection>? Activities { get; init; }
    public UpdateField<RelatedTagsCollection>? Tags { get; init; }
}