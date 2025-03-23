using System.Text.Json;
using Freem.Entities.Activities.Identifiers;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Http.Requests.Entities.Extensions;
using Freem.Web.Api.Public.Client.Implementations.Base;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.Activities;

namespace Freem.Web.Api.Public.Client.Implementations;

public sealed class ActivitiesClient : BaseClient
{
    private readonly JsonSerializerOptions _options;

    public ActivitiesClient(IHttpClient client, JsonSerializerOptions options, TokenLoader tokenLoader)
        : base(client, options, tokenLoader)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;
    }

    public Task<ClientResult<CreateActivityResponse>> CreateAsync(CreateActivityRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/activities")
            .WithJsonBody(body, _options);
        
        return SendAuthorizedAsync<CreateActivityResponse>(request, cancellationToken);
    }

    public Task<ClientResult> UpdateAsync(
        ActivityIdentifier id,
        UpdateActivityRequest body, 
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Put($"api/v1/activities/{id}")
            .WithJsonBody(body, _options);

        return SendAuthorizedAsync(request, cancellationToken);
    }

    public Task<ClientResult> RemoveAsync(ActivityIdentifier id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Delete($"api/v1/activities/{id}");
        
        return SendAuthorizedAsync(request, cancellationToken);
    }

    public Task<ClientResult> ArchiveAsync(ActivityIdentifier id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post($"api/v1/activities/{id}/archive");
        
        return SendAuthorizedAsync(request, cancellationToken);
    }

    public Task<ClientResult> UnarchiveAsync(ActivityIdentifier id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post($"api/v1/activities/{id}/unarchive");
        
        return SendAuthorizedAsync(request, cancellationToken);
    }

    public Task<ClientResult<ActivityResponse>> GetAsync(
        ActivityIdentifier id, 
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/activities/{id}");
        
        return SendAuthorizedAsync<ActivityResponse>(request, cancellationToken);
    }

    public Task<ClientResult<IAsyncEnumerable<ActivityResponse>>> ListAsync(
        ListActivityRequest query, 
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/activities")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());
        
        return SendAuthorizedAsync<IAsyncEnumerable<ActivityResponse>>(request, cancellationToken);
    }
}