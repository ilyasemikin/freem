using Freem.Domain.Activities;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Time.Models;

namespace Freem.Api.Public.Contracts.DTO.Records;

public sealed class CreateRecordRequest
{
    public DateTimePeriod Period { get; }
    
    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    
    public IReadOnlyRelatedIdsCollection<ActivityId> Activities { get; }
    public IReadOnlyRelatedIdsCollection<TagId> Tags { get; }

    public CreateRecordRequest(
        DateTimePeriod period, 
        IReadOnlyRelatedIdsCollection<ActivityId> activities,
        IReadOnlyRelatedIdsCollection<TagId>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(period);
        ArgumentNullException.ThrowIfNull(activities);
        
        Period = period;
        Activities = activities;
        Tags = tags ?? RelatedTagIdsCollection.Empty;
    }
}