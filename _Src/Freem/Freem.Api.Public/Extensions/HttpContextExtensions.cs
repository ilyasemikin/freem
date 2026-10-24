using Freem.Domain.Users;
using Freem.Api.Public.Authentication;
using Freem.Api.Public.UseCases.Context;

namespace Freem.Api.Public.Extensions;

internal static class HttpContextExtensions
{
    extension(HttpContext context)
    {
        public ApiAuthorizedHandlerContext ToAuthorizedHandlerContext()
        {
            var userId = context.GetUserId();
            return new ApiAuthorizedHandlerContext(userId);
        }

        public ApiAnonymousHandlerContext ToAnonymousHandlerContext()
        {
            return new ApiAnonymousHandlerContext();
        }
        
        private UserId GetUserId()
        {
            // The claim is guaranteed by the EgoHub bearer authentication, which rejects tokens without a valid subject.
            var value = context.User.FindFirst(AuthenticationClaimTypes.UserId)?.Value;
            return UserId.TryParse(value, null, out var userId)
                ? userId
                : throw new InvalidOperationException("No user id claim found");
        }
    }
}
