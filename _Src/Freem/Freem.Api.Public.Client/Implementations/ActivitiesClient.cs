using Freem.Api.Public.Contracts.V1.Activities.Dtos;
using System.Text.Json;
using Freem.Domain.Activities;
using Freem.Api.Public.Client.Implementations.Base;
using Freem.Api.Public.Contracts.V1.Activities;
using Utilities.Http;
using Utilities.Http.Errors.Abstractions;
using Utilities.Http.Extensions;
using Utilities.Primitives;

namespace Freem.Api.Public.Client.Implementations;

public sealed class ActivitiesClient : BaseClient
{
    private readonly JsonSerializerOptions _options;

    public ActivitiesClient(IHttpClient client, JsonSerializerOptions options, ClientSession session)
        : base(client, options, session)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;
    }

    public Task<Result<ActivityCreateResponse, IClientError>> CreateAsync(ActivityCreateRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/activities")
            .WithJsonBody(body, _options);
        
        return SendForUserAsync<ActivityCreateResponse>(request, cancellationToken);
    }

    public Task<Result<IClientError>> UpdateAsync(
        ActivityId id,
        ActivityUpdateRequest body, 
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Put($"api/v1/activities/{id}")
            .WithJsonBody(body, _options);

        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<IClientError>> RemoveAsync(ActivityId id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Delete($"api/v1/activities/{id}");
        
        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<IClientError>> ArchiveAsync(ActivityId id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post($"api/v1/activities/{id}/archive");
        
        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<IClientError>> UnarchiveAsync(ActivityId id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post($"api/v1/activities/{id}/unarchive");
        
        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<ActivityResponse, IClientError>> GetAsync(
        ActivityId id, 
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/activities/{id}");
        
        return SendForUserAsync<ActivityResponse>(request, cancellationToken);
    }

    public Task<Result<ActivityListResponse, IClientError>> ListAsync(
        ActivityListRequest query, 
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("api/v1/activities")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());
        
        return SendForUserAsync<ActivityListResponse>(request, cancellationToken);
    }
}