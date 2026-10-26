using Freem.Domain.Activities;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;

namespace Freem.Api.Public.Contracts.V1.Records.Running;

/// <summary>Request body for updating the running record. Fields that are omitted stay unchanged.</summary>
public sealed class RunningRecordUpdateRequest
{
    /// <summary>New record name; a null value clears it.</summary>
    public UpdateField<RecordName?>? Name { get; init; }
    /// <summary>New record description; a null value clears it.</summary>
    public UpdateField<RecordDescription?>? Description { get; init; }
    
    /// <summary>New full set of activity ids; replaces the current one.</summary>
    public UpdateField<IReadOnlyRelatedIdsCollection<ActivityId>>? Activities { get; init; }
    /// <summary>New full set of tag ids; replaces the current one.</summary>
    public UpdateField<IReadOnlyRelatedIdsCollection<TagId>>? Tags { get; init; }

    /// <summary>New start of tracking; must not be in the future.</summary>
    public UpdateField<DateTimeOffset>? StartAt { get; init; }

    /// <summary>Whether at least one field is set.</summary>
    public bool HasChanges()
    {
        return
            Name is not null ||
            Description is not null ||
            Activities is not null ||
            Tags is not null ||
            StartAt is not null;
    }
}