using Freem.Domain.Activities;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;

namespace Freem.Api.Public.Contracts.V1.Records;

/// <summary>Request body for creating a finished record.</summary>
public sealed class RecordCreateRequest
{
    /// <summary>Periods of the record: at least one, they must not overlap.</summary>
    public IReadOnlyList<RecordPeriodItem> Periods { get; }
    
    /// <summary>Optional record name.</summary>
    public RecordName? Name { get; init; }
    /// <summary>Optional record description.</summary>
    public RecordDescription? Description { get; init; }
    
    /// <summary>Ids of the activities the time was spent on.</summary>
    public IReadOnlyRelatedIdsCollection<ActivityId> Activities { get; }
    /// <summary>Ids of the tags to attach. Empty when omitted.</summary>
    public IReadOnlyRelatedIdsCollection<TagId> Tags { get; }

    public RecordCreateRequest(
        IReadOnlyList<RecordPeriodItem> periods, 
        IReadOnlyRelatedIdsCollection<ActivityId> activities,
        IReadOnlyRelatedIdsCollection<TagId>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(periods);
        ArgumentNullException.ThrowIfNull(activities);
        
        Periods = periods;
        Activities = activities;
        Tags = tags ?? RelatedTagIdsCollection.Empty;
    }
}