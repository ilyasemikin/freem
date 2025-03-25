using System.Text.Json;
using Freem.Entities.Activities.Identifiers;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Http.Requests.Entities.Extensions;
using Freem.Web.Api.Public.Client;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.Activities;
using Freem.Web.Api.Public.SyncClient.Implementations.Base;

namespace Freem.Web.Api.Public.SyncClient.Implementations;

public sealed class ActivitiesSyncClient : BaseSyncClient
{
    private readonly JsonSerializerOptions _options;
    
    public ActivitiesSyncClient(IHttpClient client, JsonSerializerOptions options, TokenLoader tokenLoader) 
        : base(client, options, tokenLoader)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;
    }

    public ClientResult<CreateActivityResponse> Create(CreateActivityRequest body)
    {
        var request = HttpRequest.Post("api/v1/activities")
            .WithJsonBody(body, _options);

        return SendAuthorized<CreateActivityResponse>(request);
    }

    public ClientResult Update(ActivityIdentifier id, UpdateActivityRequest body)
    {
        var request = HttpRequest.Put($"api/v1/activities/{id}")
            .WithJsonBody(body, _options);
        
        return SendAuthorized(request);
    }

    public ClientResult Remove(ActivityIdentifier id)
    {
        var request = HttpRequest.Delete($"api/v1/activities/{id}");
        
        return SendAuthorized(request);
    }

    public ClientResult Archive(ActivityIdentifier id)
    {
        var request = HttpRequest.Post($"api/v1/activities/{id}/archive");
        
        return SendAuthorized(request);
    }

    public ClientResult Unarchive(ActivityIdentifier id)
    {
        var request = HttpRequest.Post($"api/v1/activities/{id}/unarchive");
        
        return SendAuthorized(request);
    }

    public ClientResult<ActivityResponse> Get(ActivityIdentifier id)
    {
        var request = HttpRequest.Get($"api/v1/activities/{id}");
        
        return SendAuthorized<ActivityResponse>(request);
    }

    public ClientResult<IAsyncEnumerable<ActivityResponse>> List(ListActivityRequest query)
    {
        var request = HttpRequest.Get($"api/v1/activities")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());
        
        return SendAuthorized<IAsyncEnumerable<ActivityResponse>>(request);
    }
}