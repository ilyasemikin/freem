using System.Text.Json;
using Freem.Entities.Activities.Identifiers;
using Freem.Web.Api.Public.Client.Implementations.Base;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.DTO.Activities;
using Utilities.Http;
using Utilities.Http.Extensions;

namespace Freem.Web.Api.Public.Client.Implementations;

public sealed class ActivitiesClient : BaseClient
{
    private readonly JsonSerializerOptions _options;

    public ActivitiesClient(IHttpClient client, JsonSerializerOptions options, ClientSession session)
        : base(client, options, session)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;
    }

    public Task<ClientResult<CreateActivityResponse>> CreateAsync(CreateActivityRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/activities")
            .WithJsonBody(body, _options);
        
        return SendForUserAsync<CreateActivityResponse>(request, cancellationToken);
    }

    public Task<ClientResult> UpdateAsync(
        ActivityIdentifier id,
        UpdateActivityRequest body, 
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Put($"api/v1/activities/{id}")
            .WithJsonBody(body, _options);

        return SendForUserAsync(request, cancellationToken);
    }

    public Task<ClientResult> RemoveAsync(ActivityIdentifier id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Delete($"api/v1/activities/{id}");
        
        return SendForUserAsync(request, cancellationToken);
    }

    public Task<ClientResult> ArchiveAsync(ActivityIdentifier id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post($"api/v1/activities/{id}/archive");
        
        return SendForUserAsync(request, cancellationToken);
    }

    public Task<ClientResult> UnarchiveAsync(ActivityIdentifier id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post($"api/v1/activities/{id}/unarchive");
        
        return SendForUserAsync(request, cancellationToken);
    }

    public Task<ClientResult<ActivityResponse>> GetAsync(
        ActivityIdentifier id, 
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/activities/{id}");
        
        return SendForUserAsync<ActivityResponse>(request, cancellationToken);
    }

    public Task<ClientResult<IAsyncEnumerable<ActivityResponse>>> ListAsync(
        ListActivityRequest query, 
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("api/v1/activities")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());
        
        return SendForUserAsync<IAsyncEnumerable<ActivityResponse>>(request, cancellationToken);
    }
}