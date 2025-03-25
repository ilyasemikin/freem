using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Models;
using Freem.Entities.Tags.Identifiers;

namespace Freem.Web.Api.Public.Contracts.Records;

public sealed class UpdateRecordRequest
{
    public UpdateField<RecordName?>? Name { get; init; }
    public UpdateField<RecordDescription?>? Description { get; init; }
    
    public UpdateField<IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier>>? Activities { get; init; }
    public UpdateField<IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier>>? Tags { get; init; }

    public bool HasChanges()
    {
        return
            Name is not null ||
            Description is not null ||
            Activities is not null ||
            Tags is not null;
    }
}