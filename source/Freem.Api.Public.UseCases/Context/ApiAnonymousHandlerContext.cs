using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Context;

/// <summary>
/// Handler context for requests made before a Freem user is known (EgoHub registration, login, session refresh).
/// </summary>
public sealed class ApiAnonymousHandlerContext : IHandlerContext
{
    public RequestId RequestId { get; }

    public ApiAnonymousHandlerContext()
        : this(RequestId.Generate())
    {
    }

    public ApiAnonymousHandlerContext(RequestId requestId)
    {
        ArgumentNullException.ThrowIfNull(requestId);

        RequestId = requestId;
    }
}
