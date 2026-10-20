using Freem.Domain.Users;
using Freem.Api.Public.Authentication;
using Freem.Api.Public.UseCases.Context;

namespace Freem.Api.Public.Extensions;

internal static class HttpContextExtensions
{
    public static UserId GetUserId(this HttpContext context)
    {
        // The claim is guaranteed by the EgoHub bearer authentication, which rejects tokens without a valid subject.
        var value = context.User.FindFirst(AuthenticationClaimTypes.UserId)?.Value;
        return UserId.TryParse(value, null, out var userId)
            ? userId
            : throw new InvalidOperationException("No user id claim found");
    }

    public static ApiAuthorizedHandlerContext ToAuthorizedHandlerContext(this HttpContext context)
    {
        var userId = context.GetUserId();
        return new ApiAuthorizedHandlerContext(userId);
    }

    public static ApiAnonymousHandlerContext ToAnonymousHandlerContext(this HttpContext context)
    {
        return new ApiAnonymousHandlerContext();
    }
}
