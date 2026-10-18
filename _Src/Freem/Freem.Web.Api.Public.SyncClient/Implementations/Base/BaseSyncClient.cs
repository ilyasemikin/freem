using System.Text.Json;
using Freem.Web.Api.Public.Client;
using Freem.Web.Api.Public.Client.Implementations.Extensions;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts;
using Utilities.Http;

namespace Freem.Web.Api.Public.SyncClient.Implementations.Base;

public abstract class BaseSyncClient
{
    private readonly IHttpClient _client;
    private readonly JsonSerializerOptions _options;
    private readonly ClientSession? _session;

    protected BaseSyncClient(IHttpClient client, JsonSerializerOptions options, ClientSession? session = null)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        
        _client = client;
        _options = options;
        _session = session;
    }

    protected ClientResult Send(HttpRequest request)
    {
        var response = SendInner(request);
        return response.ToClientResult();
    }

    public ClientResult SendForUser(HttpRequest request)
    {
        var response = SendForUserInner(request);
        return response.ToClientResult();
    }

    protected ClientResult<T> Send<T>(HttpRequest request)
    {
        var response = SendInner(request);
        return response.ToClientResultAsync<T>(_options)
            .GetAwaiter()
            .GetResult();
    }

    public ClientResult<T> SendForUser<T>(HttpRequest request)
    {
        var response = SendForUserInner(request);
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

    private HttpResponse SendForUserInner(HttpRequest request)
    {
        if (_session is null || !_session.TryGet(out var accessToken))
            throw new InvalidOperationException("Client session is not set");

        request = request.WithHeader(HeaderNames.Authorization, $"Bearer {accessToken}");
        request = PopulateRequest(request);

        return _client.SendAsync(request)
            .GetAwaiter()
            .GetResult();
    }

    private static HttpRequest PopulateRequest(HttpRequest request)
    {
        const string client = "C# Freem.Web.Api.Public.Client";
        return request.WithHeader(HeaderNames.ClientName, client);
    }
}