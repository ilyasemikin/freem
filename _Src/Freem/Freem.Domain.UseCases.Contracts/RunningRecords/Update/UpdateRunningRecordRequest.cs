using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.RunningRecords.Update;

public sealed class UpdateRunningRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.RunningRecords.Update;

    public UpdateField<RecordName?>? Name { get; init; }
    public UpdateField<RecordDescription?>? Description { get; init; }
    
    public UpdateField<RelatedActivityIdsCollection>? Activities { get; init; }
    public UpdateField<RelatedTagIdsCollection>? Tags { get; init; }

    public bool HasChanges()
    {
        return 
            Name is not null ||
            Description is not null ||
            Activities is not null ||
            Tags is not null;
    }
}