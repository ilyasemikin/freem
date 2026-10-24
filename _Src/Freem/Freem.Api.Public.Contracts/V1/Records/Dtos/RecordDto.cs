using Freem.Domain.Activities;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;

namespace Freem.Api.Public.Contracts.V1.Records.Dtos;

/// <summary>A finished record.</summary>
public sealed class RecordDto
{
    /// <summary>Record id.</summary>
    public RecordId Id { get; }
    
    /// <summary>Record name, if set.</summary>
    public RecordName? Name { get; init; }
    /// <summary>Record description, if set.</summary>
    public RecordDescription? Description { get; init; }
    
    /// <summary>Periods of the record in ascending order of start.</summary>
    public IReadOnlyList<RecordPeriodDto> Periods { get; }
    
    /// <summary>Ids of the activities the time was spent on.</summary>
    public IReadOnlyRelatedIdsCollection<ActivityId> Activities { get; }
    /// <summary>Ids of the attached tags.</summary>
    public IReadOnlyRelatedIdsCollection<TagId> Tags { get; }

    public RecordDto(
        RecordId id,
        IReadOnlyList<RecordPeriodDto> periods,
        IReadOnlyRelatedIdsCollection<ActivityId> activities,
        IReadOnlyRelatedIdsCollection<TagId>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(periods);
        ArgumentNullException.ThrowIfNull(activities);
        
        Id = id;
        Periods = periods;
        Activities = activities;
        Tags = tags ?? RelatedTagIdsCollection.Empty;
    }
}