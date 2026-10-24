using Freem.Api.Public.Contracts.V1.Tags.Dtos;
using System.Text.Json;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Api.Public.Client.Implementations.Base;
using Freem.Api.Public.Contracts.V1.Tags;
using Utilities.Http;
using Utilities.Http.Errors.Abstractions;
using Utilities.Http.Extensions;
using Utilities.Primitives;

namespace Freem.Api.Public.Client.Implementations;

public sealed class TagsClient : BaseClient
{
    private readonly JsonSerializerOptions _options;
    
    public TagsClient(IHttpClient client, JsonSerializerOptions options, ClientSession session) 
        : base(client, options, session)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;
    }

    public Task<Result<TagCreateResponse, IClientError>> CreateAsync(TagCreateRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/tags")
            .WithJsonBody(body, _options);

        return SendForUserAsync<TagCreateResponse>(request, cancellationToken);
    }

    public Task<Result<IClientError>> UpdateAsync(
        TagId id,
        TagUpdateRequest body, 
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Put($"api/v1/tags/{id}")
            .WithJsonBody(body, _options);

        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<IClientError>> RemoveAsync(TagId id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Delete($"api/v1/tags/{id}");
        
        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<TagResponse, IClientError>> GetAsync(TagId id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/tags/{id}");
        
        return SendForUserAsync<TagResponse>(request, cancellationToken);
    }

    public Task<Result<TagResponse, IClientError>> GetAsync(TagName name, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/tags/by-name/{name}");
        
        return SendForUserAsync<TagResponse>(request, cancellationToken);
    }

    public Task<Result<TagListResponse, IClientError>> ListAsync(
        TagListRequest query, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/tags")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());
        
        return SendForUserAsync<TagListResponse>(request, cancellationToken);
    }
}