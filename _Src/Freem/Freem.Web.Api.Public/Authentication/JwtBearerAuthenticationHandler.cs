using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using Freem.Entities.Tokens.JWT.Implementations.AccessTokens;
using Microsoft.Extensions.Options;

namespace Freem.Web.Api.Public.Authentication;

public sealed class JwtBearerAuthenticationHandler : JwtAuthenticationHandlerBase<JwtBearerAuthenticationOptions>
{
    private const string AuthenticationType = "JWT Bearer";
    private const string BearerPrefix = "Bearer ";
    
    public JwtBearerAuthenticationHandler(
        IOptionsMonitor<JwtBearerAuthenticationOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder,
        AccessTokenValidator validator)
        : base(options, logger, encoder, validator, AuthenticationType)
    {
    }

    protected override bool TryGetToken([NotNullWhen(true)] out string? token)
    {
        token = null;
        
        var authorization = Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authorization) || !authorization.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase))
            return false;
        
        token = authorization[BearerPrefix.Length..].Trim();
        return !string.IsNullOrWhiteSpace(token);
    }
}