using System.Text.Json;
using Freem.Domain.Activities;
using Freem.Web.Api.Public.Client;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.DTO.Activities;
using Freem.Web.Api.Public.SyncClient.Implementations.Base;
using Utilities.Http;
using Utilities.Http.Extensions;

namespace Freem.Web.Api.Public.SyncClient.Implementations;

public sealed class ActivitiesSyncClient : BaseSyncClient
{
    private readonly JsonSerializerOptions _options;
    
    public ActivitiesSyncClient(IHttpClient client, JsonSerializerOptions options, ClientSession session) 
        : base(client, options, session)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;
    }

    public ClientResult<CreateActivityResponse> Create(CreateActivityRequest body)
    {
        var request = HttpRequest.Post("api/v1/activities")
            .WithJsonBody(body, _options);

        return SendForUser<CreateActivityResponse>(request);
    }

    public ClientResult Update(ActivityId id, UpdateActivityRequest body)
    {
        var request = HttpRequest.Put($"api/v1/activities/{id}")
            .WithJsonBody(body, _options);
        
        return SendForUser(request);
    }

    public ClientResult Remove(ActivityId id)
    {
        var request = HttpRequest.Delete($"api/v1/activities/{id}");
        
        return SendForUser(request);
    }

    public ClientResult Archive(ActivityId id)
    {
        var request = HttpRequest.Post($"api/v1/activities/{id}/archive");
        
        return SendForUser(request);
    }

    public ClientResult Unarchive(ActivityId id)
    {
        var request = HttpRequest.Post($"api/v1/activities/{id}/unarchive");
        
        return SendForUser(request);
    }

    public ClientResult<ActivityResponse> Get(ActivityId id)
    {
        var request = HttpRequest.Get($"api/v1/activities/{id}");
        
        return SendForUser<ActivityResponse>(request);
    }

    public ClientResult<IAsyncEnumerable<ActivityResponse>> List(ListActivityRequest query)
    {
        var request = HttpRequest.Get($"api/v1/activities")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());
        
        return SendForUser<IAsyncEnumerable<ActivityResponse>>(request);
    }
}