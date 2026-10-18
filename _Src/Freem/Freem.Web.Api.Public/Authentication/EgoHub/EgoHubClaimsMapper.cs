using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Freem.Web.Api.Public.Authentication.EgoHub;

// The Freem user identifier is the subject of the validated EgoHub access token.
internal static class EgoHubClaimsMapper
{
    public static bool TryMapUserIdClaim(ClaimsPrincipal principal, [NotNullWhen(true)] out Claim? claim)
    {
        var subject = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrWhiteSpace(subject))
        {
            claim = null;
            return false;
        }

        claim = new Claim(AuthenticationClaimTypes.UserId, subject);
        return true;
    }
}
