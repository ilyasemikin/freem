using Freem.Domain.Storage.Models.Filters.Abstractions;
using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Users;

namespace Freem.Domain.Storage.Repositories.Records;

public sealed class RecordByUserFilter : ILimitFilter, IOffsetFilter
{
    public required Limit Limit { get; init; }
    public required Offset Offset { get; init; }
    
    public UserId UserId { get; } 
    
    public RecordByUserFilter(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserId = userId;
    }
}