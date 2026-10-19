using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;

namespace Freem.Entities.UseCases.Contracts.RunningRecords.Start;

public sealed class StartRunningRecordRequest
{
    private readonly RelatedTagsIdentifiersCollection _tags = RelatedTagsIdentifiersCollection.Empty;
    
    public DateTimeOffset StartAt { get; }
    
    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    
    public RelatedActivitiesIdentifiersCollection Activities { get; }

    public RelatedTagsIdentifiersCollection Tags
    {
        get => _tags;
        init
        {
            ArgumentNullException.ThrowIfNull(value);

            _tags = value;
        }
    }
    
    public StartRunningRecordRequest(DateTimeOffset startAt, RelatedActivitiesIdentifiersCollection activities)
    {
        ArgumentNullException.ThrowIfNull(activities);
        
        StartAt = startAt;
        Activities = activities;
    }
}