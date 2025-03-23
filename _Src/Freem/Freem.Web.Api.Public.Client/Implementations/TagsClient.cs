using System.Text.Json;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Tags.Models;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Http.Requests.Entities.Extensions;
using Freem.Web.Api.Public.Client.Implementations.Base;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.Tags;

namespace Freem.Web.Api.Public.Client.Implementations;

public sealed class TagsClient : BaseClient
{
    private readonly JsonSerializerOptions _options;
    
    public TagsClient(IHttpClient client, JsonSerializerOptions options, TokenLoader tokenLoader) 
        : base(client, options, tokenLoader)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;
    }

    public Task<ClientResult<CreateTagResponse>> CreateAsync(CreateTagRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/tags")
            .WithJsonBody(body, _options);

        return SendAuthorizedAsync<CreateTagResponse>(request, cancellationToken);
    }

    public Task<ClientResult> UpdateAsync(
        TagIdentifier id,
        UpdateTagRequest body, 
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Put($"api/v1/tags/{id}")
            .WithJsonBody(body, _options);

        return SendAuthorizedAsync(request, cancellationToken);
    }

    public Task<ClientResult> RemoveAsync(TagIdentifier id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Delete($"api/v1/tags/{id}");
        
        return SendAuthorizedAsync(request, cancellationToken);
    }

    public Task<ClientResult<TagResponse>> GetAsync(TagIdentifier id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/tags/{id}");
        
        return SendAuthorizedAsync<TagResponse>(request, cancellationToken);
    }

    public Task<ClientResult<TagResponse>> GetAsync(TagName name, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/tags/by-name/{name}");
        
        return SendAuthorizedAsync<TagResponse>(request, cancellationToken);
    }

    public Task<ClientResult<IAsyncEnumerable<TagResponse>>> ListAsync(
        ListTagRequest query, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/tags")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());
        
        return SendAuthorizedAsync<IAsyncEnumerable<TagResponse>>(request, cancellationToken);
    }
}