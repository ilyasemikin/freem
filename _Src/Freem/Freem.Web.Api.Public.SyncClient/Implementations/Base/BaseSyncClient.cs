using System.Text.Json;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Web.Api.Public.Client;
using Freem.Web.Api.Public.Client.Implementations.Extensions;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts;

namespace Freem.Web.Api.Public.SyncClient.Implementations.Base;

public abstract class BaseSyncClient
{
    private readonly IHttpClient _client;
    private readonly JsonSerializerOptions _options;
    private readonly CurrentUser? _currentUser;

    protected BaseSyncClient(IHttpClient client, JsonSerializerOptions options, CurrentUser? currentUser = null)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        
        _client = client;
        _options = options;
        _currentUser = currentUser;
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
        if (_currentUser is null || !_currentUser.TryGet(out var userId))
            throw new InvalidOperationException("Current user is not set");

        request = request.WithQueryParameter(QueryParameterNames.UserId, userId.ToString());
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