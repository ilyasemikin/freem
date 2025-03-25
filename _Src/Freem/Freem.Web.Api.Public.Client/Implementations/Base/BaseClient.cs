using System.Net;
using System.Text.Json;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Web.Api.Public.Client.Constants;
using Freem.Web.Api.Public.Client.Implementations.Extensions;
using Freem.Web.Api.Public.Client.Models;

namespace Freem.Web.Api.Public.Client.Implementations.Base;

public abstract class BaseClient
{
    private readonly IHttpClient _client;
    private readonly JsonSerializerOptions _options;
    private readonly TokenLoader? _tokenLoader;

    protected BaseClient(IHttpClient client, JsonSerializerOptions options, TokenLoader? tokenLoader = null)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        
        _client = client;
        _options = options;
        _tokenLoader = tokenLoader;
    }

    protected async Task<ClientResult> SendAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        var response = await SendInnerAsync(request, cancellationToken);
        return response.ToClientResult();
    }

    protected async Task<ClientResult> SendAuthorizedAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        var response = await SendAuthorizedInnerAsync(request, cancellationToken);
        return response.ToClientResult();
    }

    protected async Task<ClientResult<T>> SendAsync<T>(HttpRequest request, CancellationToken cancellationToken = default)
    {
        var response = await SendInnerAsync(request, cancellationToken);
        return await response.ToClientResultAsync<T>(_options);
    }

    protected async Task<ClientResult<T>> SendAuthorizedAsync<T>(HttpRequest request, CancellationToken cancellationToken = default)
    {
        var response = await SendAuthorizedInnerAsync(request, cancellationToken);
        return await response.ToClientResultAsync<T>(_options);
    }

    private async Task<HttpResponse> SendInnerAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        request = PopulateRequest(request);
        
        return await _client.SendAsync(request, cancellationToken);
    }
    
    private async Task<HttpResponse> SendAuthorizedInnerAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        if (_tokenLoader is null || !_tokenLoader.TryGet(out var tokens))
            throw new InvalidOperationException(); 
        
        request = PopulateAuthorizedRequest(request, tokens.AccessToken);
        
        var response = await _client.SendAsync(request, cancellationToken);
        if (response.StatusCode is HttpStatusCode.Unauthorized)
        {
            var refreshed = await _tokenLoader.TryRefreshAsync(cancellationToken);
            if (!refreshed)
                throw new InvalidOperationException();
            
            response = await _client.SendAsync(request, cancellationToken);
        }

        return response;
    }
    
    private static HttpRequest PopulateRequest(HttpRequest request)
    {
        return request.WithHeader(ClientHeadersNames.ClientVersion, ClientVersion.Version10);
    }

    private static HttpRequest PopulateAuthorizedRequest(HttpRequest request, string token)
    {
        return request
            .WithHeader(ClientHeadersNames.ClientVersion, ClientVersion.Version10)
            .WithHeader("Authorization", $"Bearer {token}");
    }
}