using System.Text.Json;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Web.Api.Public.Client;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.DTO.Tags;
using Freem.Web.Api.Public.SyncClient.Implementations.Base;
using Utilities.Http;
using Utilities.Http.Extensions;

namespace Freem.Web.Api.Public.SyncClient.Implementations;

public sealed class TagsSyncClient : BaseSyncClient
{
    private readonly JsonSerializerOptions _options;
    
    public TagsSyncClient(IHttpClient client, JsonSerializerOptions options, ClientSession session) 
        : base(client, options, session)
    {
        _options = options;
    }

    public ClientResult<CreateTagResponse> Create(CreateTagRequest body)
    {
        var request = HttpRequest.Post("api/v1/tags")
            .WithJsonBody(body, _options);

        return SendForUser<CreateTagResponse>(request);
    }

    public ClientResult Update(TagId id, UpdateTagRequest body)
    {
        var request = HttpRequest.Put($"api/v1/tags/{id}")
            .WithJsonBody(body, _options);
        
        return SendForUser(request);
    }

    public ClientResult Remove(TagId id)
    {
        var request = HttpRequest.Delete($"api/v1/tags/{id}");
        
        return SendForUser(request);
    }

    public ClientResult<TagResponse> Get(TagId id)
    {
        var request = HttpRequest.Get($"api/v1/tags/{id}");
        
        return SendForUser<TagResponse>(request);
    }

    public ClientResult<TagResponse> Get(TagName name)
    {
        var request = HttpRequest.Get($"api/v1/tags/by-name/{name}");
        
        return SendForUser<TagResponse>(request);
    }

    public ClientResult<IAsyncEnumerable<TagResponse>> List(ListTagRequest query)
    {
        var request = HttpRequest.Get($"api/v1/tags")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());
        
        return SendForUser<IAsyncEnumerable<TagResponse>>(request);
    }
}