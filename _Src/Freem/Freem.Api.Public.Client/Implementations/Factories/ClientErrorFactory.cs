using System.Text.Json;
using Freem.Api.Public.Client.Errors;
using Freem.Api.Public.Contracts.DTO;
using Utilities.Http;
using Utilities.Http.Content.Extensions;
using Utilities.Http.Errors;
using Utilities.Http.Errors.Abstractions;

namespace Freem.Api.Public.Client.Implementations.Factories;

internal sealed class ClientErrorFactory
{
    private readonly JsonSerializerOptions _options;

    public ClientErrorFactory(JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options;
    }

    public async Task<IClientError> CreateAsync(HttpResponse response, CancellationToken cancellationToken = default)
    {
        try
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>(_options, cancellationToken);
            return errorResponse is not null
                ? new ClientError(response, errorResponse.Error)
                : new HttpClientError(response);
        }
        catch
        {
            // Responses without an ErrorResponse body (e.g. the authentication challenge) keep only the status code.
            return new HttpClientError(response);
        }
    }
}
