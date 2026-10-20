using Freem.Domain.Activities;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Time.Models;
using Freem.Api.Public.Contracts.Models;

namespace Freem.Api.Public.Contracts.DTO.Records;

public sealed class UpdateRecordRequest
{
    public UpdateField<DateTimePeriod>? Period { get; init; }

    public UpdateField<RecordName?>? Name { get; init; }
    public UpdateField<RecordDescription?>? Description { get; init; }
    
    public UpdateField<IReadOnlyRelatedIdsCollection<ActivityId>>? Activities { get; init; }
    public UpdateField<IReadOnlyRelatedIdsCollection<TagId>>? Tags { get; init; }

    public bool HasChanges()
    {
        return
            Period is not null ||
            Name is not null ||
            Description is not null ||
            Activities is not null ||
            Tags is not null;
    }
}