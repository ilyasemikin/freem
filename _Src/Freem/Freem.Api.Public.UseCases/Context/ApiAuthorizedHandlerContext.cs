using Freem.Domain.Users;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Context;

public sealed class ApiAuthorizedHandlerContext : IHandlerContext
{
    public UserId UserId { get; }
    public RequestId RequestId { get; }

    public ApiAuthorizedHandlerContext(UserId userId)
        : this(userId, RequestId.Generate())
    {
    }

    public ApiAuthorizedHandlerContext(UserId userId, RequestId requestId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(requestId);

        UserId = userId;
        RequestId = requestId;
    }
}
