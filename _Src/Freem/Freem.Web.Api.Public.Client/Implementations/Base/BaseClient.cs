using System.Text.Json;
using Freem.Web.Api.Public.Client.Implementations.Extensions;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts;
using Utilities.Http;

namespace Freem.Web.Api.Public.Client.Implementations.Base;

public abstract class BaseClient
{
    private readonly IHttpClient _client;
    private readonly JsonSerializerOptions _options;
    private readonly ClientSession? _session;

    protected BaseClient(IHttpClient client, JsonSerializerOptions options, ClientSession? session = null)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        
        _client = client;
        _options = options;
        _session = session;
    }

    protected async Task<ClientResult> SendAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        var response = await SendInnerAsync(request, cancellationToken);
        return response.ToClientResult();
    }

    protected async Task<ClientResult> SendForUserAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        var response = await SendForUserInnerAsync(request, cancellationToken);
        return response.ToClientResult();
    }

    protected async Task<ClientResult<T>> SendAsync<T>(HttpRequest request, CancellationToken cancellationToken = default)
    {
        var response = await SendInnerAsync(request, cancellationToken);
        return await response.ToClientResultAsync<T>(_options);
    }

    protected async Task<ClientResult<T>> SendForUserAsync<T>(HttpRequest request, CancellationToken cancellationToken = default)
    {
        var response = await SendForUserInnerAsync(request, cancellationToken);
        return await response.ToClientResultAsync<T>(_options);
    }

    private async Task<HttpResponse> SendInnerAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        request = PopulateRequest(request);
        
        return await _client.SendAsync(request, cancellationToken);
    }
    
    private async Task<HttpResponse> SendForUserInnerAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        if (_session is null || !_session.TryGet(out var accessToken))
            throw new InvalidOperationException("Client session is not set");
        
        request = request.WithHeader(HeaderNames.Authorization, $"Bearer {accessToken}");
        request = PopulateRequest(request);
        
        return await _client.SendAsync(request, cancellationToken);
    }
    
    private static HttpRequest PopulateRequest(HttpRequest request)
    {
        const string client = "C# Freem.Web.Api.Public.Client";
        return request.WithHeader(HeaderNames.ClientName, client);
    }
}