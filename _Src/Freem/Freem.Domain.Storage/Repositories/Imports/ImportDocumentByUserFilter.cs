using Freem.Domain.Storage.Models.Filters.Abstractions;
using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Users;

namespace Freem.Domain.Storage.Repositories.Imports;

public sealed class ImportDocumentByUserFilter : ILimitFilter, IOffsetFilter
{
    public required Limit Limit { get; init; }
    public required Offset Offset { get; init; }

    public UserId UserId { get; }

    public ImportDocumentByUserFilter(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserId = userId;
    }
}
