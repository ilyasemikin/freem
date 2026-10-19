using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags.Identifiers;
using Freem.Web.Api.Public.Contracts.Models;

namespace Freem.Web.Api.Public.Contracts.DTO.Records.Running;

public sealed class UpdateRunningRecordRequest
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