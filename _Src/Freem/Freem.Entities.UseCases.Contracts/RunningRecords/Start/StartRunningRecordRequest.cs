using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;

namespace Freem.Entities.UseCases.Contracts.RunningRecords.Start;

public sealed class StartRunningRecordRequest
{
    private readonly RelatedTagIdsCollection _tags = RelatedTagIdsCollection.Empty;
    
    public DateTimeOffset StartAt { get; }
    
    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    
    public RelatedActivityIdsCollection Activities { get; }

    public RelatedTagIdsCollection Tags
    {
        get => _tags;
        init
        {
            ArgumentNullException.ThrowIfNull(value);

            _tags = value;
        }
    }
    
    public StartRunningRecordRequest(DateTimeOffset startAt, RelatedActivityIdsCollection activities)
    {
        ArgumentNullException.ThrowIfNull(activities);
        
        StartAt = startAt;
        Activities = activities;
    }
}