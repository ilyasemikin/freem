using System.Text.Json;
using Freem.Domain.Activities;
using Freem.Api.Public.Client.Implementations.Base;
using Freem.Api.Public.Contracts.DTO.Activities;
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

    public Task<Result<CreateActivityResponse, IClientError>> CreateAsync(CreateActivityRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/activities")
            .WithJsonBody(body, _options);
        
        return SendForUserAsync<CreateActivityResponse>(request, cancellationToken);
    }

    public Task<Result<IClientError>> UpdateAsync(
        ActivityId id,
        UpdateActivityRequest body, 
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

    public Task<Result<IReadOnlyList<ActivityResponse>, IClientError>> ListAsync(
        ListActivityRequest query, 
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("api/v1/activities")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());
        
        return SendForUserAsync<IReadOnlyList<ActivityResponse>>(request, cancellationToken);
    }
}