using Freem.Domain.Activities;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Time.Models;

namespace Freem.Web.Api.Public.Contracts.DTO.Records;

public sealed class RecordResponse
{
    public RecordId Id { get; }
    
    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    
    public DateTimePeriod Period { get; }
    
    public IReadOnlyRelatedIdsCollection<ActivityId> Activities { get; }
    public IReadOnlyRelatedIdsCollection<TagId> Tags { get; }

    public RecordResponse(
        RecordId id,
        DateTimePeriod period,
        IReadOnlyRelatedIdsCollection<ActivityId> activities,
        IReadOnlyRelatedIdsCollection<TagId>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(period);
        ArgumentNullException.ThrowIfNull(activities);
        
        Id = id;
        Period = period;
        Activities = activities;
        Tags = tags ?? RelatedTagIdsCollection.Empty;
    }
}