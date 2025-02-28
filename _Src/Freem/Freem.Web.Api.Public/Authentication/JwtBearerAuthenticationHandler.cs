using System.Security.Claims;
using System.Text.Encodings.Web;
using Freem.Entities.Tokens.JWT.Implementations.AccessTokens;
using Freem.Entities.Tokens.JWT.Implementations.AccessTokens.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Freem.Web.Api.Public.Authentication;

public sealed class JwtBearerAuthenticationHandler : AuthenticationHandler<JwtBearerAuthenticationOptions>
{
    private readonly AccessTokenValidator _validator;

    private const string BearerPrefix = "Bearer ";
    
    public const string AuthenticationScheme = "JwtAuthentication";
    
    public JwtBearerAuthenticationHandler(
        IOptionsMonitor<JwtBearerAuthenticationOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder,
        AccessTokenValidator validator)
        : base(options, logger, encoder)
    {
        _validator = validator;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorization = Request.Headers.Authorization.ToString();
        
        if (string.IsNullOrWhiteSpace(authorization) || !authorization.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase))
            return AuthenticateResult.NoResult();
        
        var token = authorization[BearerPrefix.Length..].Trim();
        if (string.IsNullOrWhiteSpace(token))
            return AuthenticateResult.NoResult();

        var result = await _validator.ValidateAsync(token);
        if (!result.IsValid)
            return AuthenticateResult.Fail(result.Exception);

        var ticket = CreateTicket(result.Properties);
        return AuthenticateResult.Success(ticket);
    }

    private static AuthenticationTicket CreateTicket(AccessTokenProperties properties)
    {
        var claims = new Claim[]
        {
            new(JwtBearerAuthenticationClaimTypes.UserId, properties.UserId),
        };

        var ci = new ClaimsIdentity(claims);
        var cp = new ClaimsPrincipal(ci);
        
        return new AuthenticationTicket(cp, AuthenticationScheme);
    }
}