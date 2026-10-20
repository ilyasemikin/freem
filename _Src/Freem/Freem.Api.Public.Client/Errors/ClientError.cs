using Utilities.Errors;
using Utilities.Http;
using Utilities.Http.Errors.Abstractions;

namespace Freem.Api.Public.Client.Errors;

/// <summary>
/// Unsuccessful API response whose body is a parsed <c>ErrorResponse</c>.
/// </summary>
public sealed class ClientError : IClientError
{
    public string Message { get; }

    public HttpResponse Response { get; }
    public Error Error { get; }

    public ClientError(HttpResponse response, Error error)
    {
        ArgumentNullException.ThrowIfNull(response);
        ArgumentNullException.ThrowIfNull(error);

        Response = response;
        Error = error;

        Message = $"Error occured. [{error.Type}]: {error.Message}";
    }
}
