using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Freem.Entities.Users;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Http.Requests.Entities.Extensions;
using Freem.Web.Api.Public.Client.Implementations.Base;
using Freem.Web.Api.Public.Contracts.Users.Tokens;

namespace Freem.Web.Api.Public.Client;

public sealed class TokenLoader : BaseClient
{
    private readonly JsonSerializerOptions _options;
    
    private readonly SemaphoreSlim _semaphore;

    [MemberNotNullWhen(true, nameof(Tokens))]
    public bool Authorized => Tokens is not null;
    public UserTokens? Tokens { get; private set; }

    public TokenLoader(IHttpClient client, JsonSerializerOptions options) 
        : base(client, options)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;
        _semaphore = new SemaphoreSlim(1, 1);
    }

    public bool TryGet([NotNullWhen(true)] out UserTokens? tokens)
    {
        _semaphore.Wait();
        
        try
        {
            tokens = Tokens;
            return tokens is not null;
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    public void Update(UserTokens tokens)
    {
        _semaphore.Wait();
        
        Tokens = tokens;
        
        _semaphore.Release();
    }

    public void Clean()
    {
        _semaphore.Wait();
        
        Tokens = null;
        
        _semaphore.Release();
    }
    
    public async Task<bool> TryRefreshAsync(CancellationToken cancellationToken = default)
    {
        var tokens = Tokens;
        if (tokens is null)
            return false;

        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            if (Tokens is null)
                return false;
            if (Tokens != tokens)
                return true;
            
            var body = new RefreshTokensRequest(Tokens.RefreshToken);
            var request = HttpRequest.Post("api/v1/user/tokens/refresh")
                .WithJsonBody(body, _options);
            
            var response = await SendAsync<RefreshTokensResponse>(request, cancellationToken);
            if (!response.Success)
                return false;

            Tokens = response.Value.Tokens;
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            _semaphore.Release();
        }

        return true;
    }
}