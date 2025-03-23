using System.Text.Json;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Tags.Models;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Http.Requests.Entities.Extensions;
using Freem.Web.Api.Public.Client;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.Tags;
using Freem.Web.Api.Public.SyncClient.Implementations.Base;

namespace Freem.Web.Api.Public.SyncClient.Implementations;

public sealed class TagsSyncClient : BaseSyncClient
{
    private readonly JsonSerializerOptions _options;
    
    public TagsSyncClient(IHttpClient client, JsonSerializerOptions options, TokenLoader tokenLoader) 
        : base(client, options, tokenLoader)
    {
        _options = options;
    }

    public ClientResult<CreateTagResponse> Create(CreateTagRequest body)
    {
        var request = HttpRequest.Post("api/v1/tags")
            .WithJsonBody(body, _options);

        return SendAuthorized<CreateTagResponse>(request);
    }

    public ClientResult Update(TagIdentifier id, UpdateTagRequest body)
    {
        var request = HttpRequest.Put($"api/v1/tags/{id}")
            .WithJsonBody(body, _options);
        
        return SendAuthorized(request);
    }

    public ClientResult Remove(TagIdentifier id)
    {
        var request = HttpRequest.Delete($"api/v1/tags/{id}");
        
        return SendAuthorized(request);
    }

    public ClientResult<TagResponse> Get(TagIdentifier id)
    {
        var request = HttpRequest.Get($"api/v1/tags/{id}");
        
        return SendAuthorized<TagResponse>(request);
    }

    public ClientResult<TagResponse> Get(TagName name)
    {
        var request = HttpRequest.Get($"api/v1/tags/by-name/{name}");
        
        return SendAuthorized<TagResponse>(request);
    }

    public ClientResult<IAsyncEnumerable<TagResponse>> List(ListTagRequest query)
    {
        var request = HttpRequest.Get($"api/v1/tags")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());
        
        return SendAuthorized<IAsyncEnumerable<TagResponse>>(request);
    }
}