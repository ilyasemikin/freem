using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.RunningRecords.Start;

public sealed class StartRunningRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.RunningRecords.Start;

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