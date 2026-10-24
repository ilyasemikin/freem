using System.Text.Json;
using Freem.Api.Public.Client.Implementations.Base;
using Freem.Api.Public.Contracts.V1.DataTransfer.Formats;
using Utilities.Http;
using Utilities.Http.Errors.Abstractions;
using Utilities.Primitives;

namespace Freem.Api.Public.Client.Implementations.DataTransfer;

public sealed class FormatsClient : BaseClient
{
    public FormatsClient(IHttpClient client, JsonSerializerOptions options, ClientSession session)
        : base(client, options, session)
    {
    }

    public Task<Result<FormatListResponse, IClientError>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("api/v1/data-transfer/formats");

        return SendForUserAsync<FormatListResponse>(request, cancellationToken);
    }
}
