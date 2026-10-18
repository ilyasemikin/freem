using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Models;
using Freem.Entities.Tags.Identifiers;
using Freem.Time.Models;
using Freem.Web.Api.Public.Contracts.Models;

namespace Freem.Web.Api.Public.Contracts.DTO.Records;

public sealed class UpdateRecordRequest
{
    public UpdateField<DateTimePeriod>? Period { get; init; }

    public UpdateField<RecordName?>? Name { get; init; }
    public UpdateField<RecordDescription?>? Description { get; init; }
    
    public UpdateField<IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier>>? Activities { get; init; }
    public UpdateField<IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier>>? Tags { get; init; }

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