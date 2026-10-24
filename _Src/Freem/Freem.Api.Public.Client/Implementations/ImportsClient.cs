using System.Text.Json;
using Freem.Api.Public.Client.Implementations.Base;
using Freem.Api.Public.Contracts.DTO.Imports;
using Freem.Domain.Formats;
using Freem.Domain.Import;
using Freem.Domain.Users.Models;
using Utilities.Http;
using Utilities.Http.Errors.Abstractions;
using Utilities.Http.Extensions;
using Utilities.Primitives;

namespace Freem.Api.Public.Client.Implementations;

public sealed class ImportsClient : BaseClient
{
    private readonly JsonSerializerOptions _options;

    public ImportsClient(IHttpClient client, JsonSerializerOptions options, ClientSession session)
        : base(client, options, session)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options;
    }

    public Task<Result<UploadImportResponse, IClientError>> UploadAsync(
        FormatKey format,
        string fileName,
        Stream content,
        TimeZoneOffset? timeZoneOffset = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(format);
        ArgumentNullException.ThrowIfNull(content);

        var fields = new List<KeyValuePair<string, string>>
        {
            new("format", format.ToString())
        };
        if (timeZoneOffset is not null)
            fields.Add(new KeyValuePair<string, string>("timeZoneOffset", timeZoneOffset.ToString()));

        var body = new ImportFormContent(fields, "file", fileName, content);
        var request = HttpRequest.Post("api/v1/imports")
            .WithBody(body);

        return SendForUserAsync<UploadImportResponse>(request, cancellationToken);
    }

    public Task<Result<IReadOnlyList<ImportResponse>, IClientError>> ListAsync(
        ListImportRequest query,
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("api/v1/imports")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());

        return SendForUserAsync<IReadOnlyList<ImportResponse>>(request, cancellationToken);
    }

    public Task<Result<ImportResponse, IClientError>> GetAsync(
        ImportDocumentId id,
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/imports/{id}");

        return SendForUserAsync<ImportResponse>(request, cancellationToken);
    }

    public Task<Result<IReadOnlyList<ImportActivityResponse>, IClientError>> ListActivitiesAsync(
        ImportDocumentId id,
        ListImportActivityRequest query,
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/imports/{id}/activities")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());

        return SendForUserAsync<IReadOnlyList<ImportActivityResponse>>(request, cancellationToken);
    }

    public Task<Result<IReadOnlyList<ImportTagResponse>, IClientError>> ListTagsAsync(
        ImportDocumentId id,
        ListImportTagRequest query,
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/imports/{id}/tags")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());

        return SendForUserAsync<IReadOnlyList<ImportTagResponse>>(request, cancellationToken);
    }

    public Task<Result<IReadOnlyList<ImportRowResponse>, IClientError>> ListRowsAsync(
        ImportDocumentId id,
        ListImportRowRequest query,
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/imports/{id}/rows")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());

        return SendForUserAsync<IReadOnlyList<ImportRowResponse>>(request, cancellationToken);
    }

    public Task<Result<IClientError>> UpdateActivityMappingAsync(
        ImportDocumentId id,
        ImportActivityId importActivityId,
        UpdateImportActivityMappingRequest body,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(body);

        var request = HttpRequest.Put($"api/v1/imports/{id}/activities/{importActivityId}")
            .WithJsonBody(body, _options);

        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<IClientError>> UpdateTagMappingAsync(
        ImportDocumentId id,
        ImportTagId importTagId,
        UpdateImportTagMappingRequest body,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(body);

        var request = HttpRequest.Put($"api/v1/imports/{id}/tags/{importTagId}")
            .WithJsonBody(body, _options);

        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<ApplyImportResponse, IClientError>> ApplyAsync(
        ImportDocumentId id,
        ApplyImportRequest body,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(body);

        var request = HttpRequest.Post($"api/v1/imports/{id}/apply")
            .WithJsonBody(body, _options);

        return SendForUserAsync<ApplyImportResponse>(request, cancellationToken);
    }

    public Task<Result<IClientError>> RemoveAsync(
        ImportDocumentId id,
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Delete($"api/v1/imports/{id}");

        return SendForUserAsync(request, cancellationToken);
    }
}
