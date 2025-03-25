using System.Net;
using System.Text.Json;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Web.Api.Public.Client;
using Freem.Web.Api.Public.Client.Constants;
using Freem.Web.Api.Public.Client.Implementations.Extensions;
using Freem.Web.Api.Public.Client.Models;

namespace Freem.Web.Api.Public.SyncClient.Implementations.Base;

public abstract class BaseSyncClient
{
    private readonly IHttpClient _client;
    private readonly JsonSerializerOptions _options;
    private readonly TokenLoader? _tokenLoader;

    protected BaseSyncClient(IHttpClient client, JsonSerializerOptions options, TokenLoader? tokenLoader = null)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        
        _client = client;
        _options = options;
        _tokenLoader = tokenLoader;
    }

    protected ClientResult Send(HttpRequest request)
    {
        var response = SendInner(request);
        return response.ToClientResult();
    }

    public ClientResult SendAuthorized(HttpRequest request)
    {
        var response = SendAuthorizedInner(request);
        return response.ToClientResult();
    }

    protected ClientResult<T> Send<T>(HttpRequest request)
    {
        var response = SendInner(request);
        return response.ToClientResultAsync<T>(_options)
            .GetAwaiter()
            .GetResult();
    }

    public ClientResult<T> SendAuthorized<T>(HttpRequest request)
    {
        var response = SendAuthorizedInner(request);
        return response.ToClientResultAsync<T>(_options)
            .GetAwaiter()
            .GetResult();
    }

    private HttpResponse SendInner(HttpRequest request)
    {
        request = PopulateRequest(request);

        return _client.SendAsync(request)
            .GetAwaiter()
            .GetResult();
    }

    private HttpResponse SendAuthorizedInner(HttpRequest request)
    {
        if (_tokenLoader is null || !_tokenLoader.TryGet(out var tokens))
            throw new InvalidOperationException();

        request = PopulateAuthorizedRequest(request, tokens.AccessToken);

        var response = _client.SendAsync(request)
            .GetAwaiter()
            .GetResult();

        if (response.StatusCode is HttpStatusCode.Unauthorized)
        {
            var refreshed = _tokenLoader.TryRefreshAsync()
                .GetAwaiter()
                .GetResult();

            if (refreshed)
                throw new InvalidCastException();
            
            response = _client.SendAsync(request)
                .GetAwaiter()
                .GetResult();
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