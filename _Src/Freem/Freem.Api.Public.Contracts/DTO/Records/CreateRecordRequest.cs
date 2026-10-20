using Freem.Domain.Activities;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Time.Models;

namespace Freem.Api.Public.Contracts.DTO.Records;

/// <summary>Request body for creating a finished record.</summary>
public sealed class CreateRecordRequest
{
    /// <summary>Start and end of the record.</summary>
    public DateTimePeriod Period { get; }
    
    /// <summary>Optional record name.</summary>
    public RecordName? Name { get; init; }
    /// <summary>Optional record description.</summary>
    public RecordDescription? Description { get; init; }
    
    /// <summary>Ids of the activities the time was spent on.</summary>
    public IReadOnlyRelatedIdsCollection<ActivityId> Activities { get; }
    /// <summary>Ids of the tags to attach. Empty when omitted.</summary>
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