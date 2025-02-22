using Freem.Entities.Models.Records;
using Freem.Entities.Relations.Collections;

namespace Freem.Entities.UseCases.Contracts.RunningRecords.Start;

public sealed class StartRunningRecordRequest
{
    private readonly RelatedTagsCollection _tags = RelatedTagsCollection.Empty;
    
    public DateTimeOffset StartAt { get; }
    
    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    
    public RelatedActivitiesCollection Activities { get; }

    public RelatedTagsCollection Tags
    {
        get => _tags;
        init
        {
            ArgumentNullException.ThrowIfNull(value);

            _tags = value;
        }
    }
    
    public StartRunningRecordRequest(DateTimeOffset startAt, RelatedActivitiesCollection activities)
    {
        ArgumentNullException.ThrowIfNull(activities);
        
        StartAt = startAt;
        Activities = activities;
    }
}