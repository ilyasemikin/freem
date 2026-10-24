using Freem.Domain.Records;
using Freem.Domain.Users;

namespace Freem.Domain.Storage.Models.Ids;

public sealed class RecordAndUserIds
{
    public RecordId RecordId { get; }
    public UserId UserId { get; }

    public RecordAndUserIds(RecordId recordId, UserId userId)
    {
        ArgumentNullException.ThrowIfNull(recordId);
        ArgumentNullException.ThrowIfNull(userId);
        
        RecordId = recordId;
        UserId = userId;
    }
}