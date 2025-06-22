using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Freem.Entities.Tokens.JWT.Implementations.AccessTokens;
using Freem.Entities.Tokens.JWT.Implementations.AccessTokens.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Freem.Web.Api.Public.Authentication;

public abstract class JwtAuthenticationHandlerBase<TOptions> : AuthenticationHandler<TOptions> 
    where TOptions : AuthenticationSchemeOptions, new()
{
    private readonly AccessTokenValidator _validator;
    private readonly string _authenticationType;
    
    protected JwtAuthenticationHandlerBase(
        IOptionsMonitor<TOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder,
        AccessTokenValidator validator, 
        string authenticationType)
        : base(options, logger, encoder)
    {
        ArgumentNullException.ThrowIfNull(validator);
        ArgumentException.ThrowIfNullOrWhiteSpace(authenticationType);
        
        _validator = validator;
        _authenticationType = authenticationType;
    }
    
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!TryGetToken(out var token))
            return AuthenticateResult.NoResult();
        
        var result = await _validator.ValidateAsync(token);
        if (!result.IsValid)
            return AuthenticateResult.Fail(result.Exception);

        var ticket = CreateTicket(result.Properties, Scheme.Name);
        return AuthenticateResult.Success(ticket);
    }

    protected abstract bool TryGetToken([NotNullWhen(true)] out string? token);
    
    private AuthenticationTicket CreateTicket(AccessTokenProperties properties, string schemeName)
    {
        var claims = new Claim[]
        {
            new(JwtAuthenticationClaimTypes.UserId, properties.UserId),
        };

        var ci = new ClaimsIdentity(claims, _authenticationType);
        var cp = new ClaimsPrincipal(ci);
        
        return new AuthenticationTicket(cp, schemeName);
    }
}