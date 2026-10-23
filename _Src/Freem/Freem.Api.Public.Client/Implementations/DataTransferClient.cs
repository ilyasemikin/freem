using System.Text.Json;
using Freem.Api.Public.Client.Implementations.Base;
using Freem.Api.Public.Contracts.DTO.DataTransfer;
using Freem.Domain.Formats;
using Utilities.Http;
using Utilities.Http.Errors.Abstractions;
using Utilities.Primitives;

namespace Freem.Api.Public.Client.Implementations;

public sealed class DataTransferClient : BaseClient
{
    public DataTransferClient(IHttpClient client, JsonSerializerOptions options, ClientSession session)
        : base(client, options, session)
    {
    }

    public Task<Result<ListFormatsResponse, IClientError>> ListFormatsAsync(
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("api/v1/formats");

        return SendForUserAsync<ListFormatsResponse>(request, cancellationToken);
    }

    public Task<Result<byte[], IClientError>> ExportAsync(
        FormatKey format,
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/export/{format}");

        return SendForUserBytesAsync(request, cancellationToken);
    }
}
