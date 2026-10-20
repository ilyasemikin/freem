using Freem.Domain.Users;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Entities.UseCases.Context;

public sealed class HandlerContext : IHandlerContext
{
    public UserId UserId { get; }
    public RequestId RequestId { get; }

    public HandlerContext(UserId userId)
        : this(userId, RequestId.Generate())
    {
    }

    public HandlerContext(UserId userId, RequestId requestId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(requestId);

        UserId = userId;
        RequestId = requestId;
    }
}
