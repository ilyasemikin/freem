using System.Text.Json;
using Freem.Api.Public.Client.Implementations.Factories;
using Freem.Api.Public.Contracts;
using Utilities.Http;
using Utilities.Http.Content.Extensions;
using Utilities.Http.Errors;
using Utilities.Http.Errors.Abstractions;
using Utilities.Primitives;

namespace Freem.Api.Public.Client.Implementations.Base;

public abstract class BaseClient
{
    private readonly IHttpClient _client;
    private readonly JsonSerializerOptions _options;
    private readonly ClientErrorFactory _errorFactory;
    private readonly ClientSession? _session;

    protected BaseClient(IHttpClient client, JsonSerializerOptions options, ClientSession? session = null)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        
        _client = client;
        _options = options;
        _errorFactory = new ClientErrorFactory(options);
        _session = session;
    }

    protected Task<Result<IClientError>> SendAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        request = PopulateRequest(request);
        return SendInnerAsync(request, cancellationToken);
    }

    protected Task<Result<IClientError>> SendForUserAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        request = PopulateUserRequest(request);
        return SendInnerAsync(request, cancellationToken);
    }

    protected Task<Result<T, IClientError>> SendAsync<T>(HttpRequest request, CancellationToken cancellationToken = default)
    {
        request = PopulateRequest(request);
        return SendInnerAsync<T>(request, cancellationToken);
    }

    protected Task<Result<T, IClientError>> SendForUserAsync<T>(HttpRequest request, CancellationToken cancellationToken = default)
    {
        request = PopulateUserRequest(request);
        return SendInnerAsync<T>(request, cancellationToken);
    }

    protected Task<Result<byte[], IClientError>> SendForUserBytesAsync(
        HttpRequest request,
        CancellationToken cancellationToken = default)
    {
        request = PopulateUserRequest(request);
        return SendInnerBytesAsync(request, cancellationToken);
    }

    private async Task<Result<IClientError>> SendInnerAsync(HttpRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _client.SendAsync(request, cancellationToken);
            if (response.Success)
                return Result<IClientError>.CreateSuccess();

            var error = await _errorFactory.CreateAsync(response, cancellationToken);
            return Result<IClientError>.CreateFailure(error);
        }
        catch (Exception exception)
        {
            return new ExceptionClientError(exception);
        }
    }

    private async Task<Result<T, IClientError>> SendInnerAsync<T>(HttpRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _client.SendAsync(request, cancellationToken);
            if (!response.Success)
            {
                var error = await _errorFactory.CreateAsync(response, cancellationToken);
                return Result<T, IClientError>.CreateFailure(error);
            }

            var value = await response.Content.ReadFromJsonAsync<T>(_options, cancellationToken);
            return value;
        }
        catch (Exception exception)
        {
            return new ExceptionClientError(exception);
        }
    }

    private async Task<Result<byte[], IClientError>> SendInnerBytesAsync(
        HttpRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _client.SendAsync(request, cancellationToken);
            if (!response.Success)
            {
                var error = await _errorFactory.CreateAsync(response, cancellationToken);
                return Result<byte[], IClientError>.CreateFailure(error);
            }

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var buffer = new MemoryStream();
            await stream.CopyToAsync(buffer, cancellationToken);
            return buffer.ToArray();
        }
        catch (Exception exception)
        {
            return new ExceptionClientError(exception);
        }
    }

    private HttpRequest PopulateUserRequest(HttpRequest request)
    {
        if (_session is null || !_session.TryGet(out var accessToken))
            throw new InvalidOperationException("Client session is not set");

        request = request.WithHeader(HeaderNames.Authorization, $"Bearer {accessToken}");
        return PopulateRequest(request);
    }
    
    private static HttpRequest PopulateRequest(HttpRequest request)
    {
        const string client = "C# Freem.Api.Public.Client";
        return request.WithHeader(HeaderNames.ClientName, client);
    }
}
