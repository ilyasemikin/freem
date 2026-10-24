using System.Text.Json;
using Freem.Api.Public.Client.Implementations.Base;
using Freem.Domain.DataTransfer.Formats;
using Utilities.Http;
using Utilities.Http.Errors.Abstractions;
using Utilities.Primitives;

namespace Freem.Api.Public.Client.Implementations.DataTransfer;

public sealed class ExportClient : BaseClient
{
    public ExportClient(IHttpClient client, JsonSerializerOptions options, ClientSession session)
        : base(client, options, session)
    {
    }

    public Task<Result<byte[], IClientError>> ExportAsync(
        FormatKey format,
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/data-transfer/exports/{format}");

        return SendForUserBytesAsync(request, cancellationToken);
    }
}
