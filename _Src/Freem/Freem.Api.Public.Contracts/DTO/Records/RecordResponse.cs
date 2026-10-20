using Freem.Domain.Activities;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Time.Models;

namespace Freem.Api.Public.Contracts.DTO.Records;

/// <summary>A finished record.</summary>
public sealed class RecordResponse
{
    /// <summary>Record id.</summary>
    public RecordId Id { get; }
    
    /// <summary>Record name, if set.</summary>
    public RecordName? Name { get; init; }
    /// <summary>Record description, if set.</summary>
    public RecordDescription? Description { get; init; }
    
    /// <summary>Start and end of the record.</summary>
    public DateTimePeriod Period { get; }
    
    /// <summary>Ids of the activities the time was spent on.</summary>
    public IReadOnlyRelatedIdsCollection<ActivityId> Activities { get; }
    /// <summary>Ids of the attached tags.</summary>
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