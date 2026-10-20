using System.Text.Json;
using Freem.Domain.Events;
using Freem.Api.Public.Client.Implementations.Base;
using Freem.Api.Public.Contracts.DTO.Events;
using Utilities.Http;
using Utilities.Http.Errors.Abstractions;
using Utilities.Primitives;

namespace Freem.Api.Public.Client.Implementations;

public sealed class EventsClient : BaseClient
{
    public EventsClient(IHttpClient client, JsonSerializerOptions options, ClientSession session) 
        : base(client, options, session)
    {
    }

    public Task<Result<EventResponse, IClientError>> GetAsync(
        EventId id,
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/events/{id}");

        return SendForUserAsync<EventResponse>(request, cancellationToken);
    }

    public Task<Result<IReadOnlyList<EventResponse>, IClientError>> ListAsync(
        ListEventRequest query, 
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("api/v1/events")
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString())
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString());

        if (query.EntityType is not null)
        {
            var name = query.EntityType.Value.ToString();
            var entityType = JsonNamingPolicy.SnakeCaseLower.ConvertName(name);
            request = request.WithQueryParameter(nameof(query.EntityType), entityType);
        }

        if (query.EntityRef is not null)
            request = request.WithQueryParameter(nameof(query.EntityRef), query.EntityRef);

        if (query.From is not null)
        {
            var from = query.From.Value.ToString("O");
            request = request.WithQueryParameter(nameof(query.From), from);
        }

        if (query.To is not null)
        {
            var to = query.To.Value.ToString("O");
            request = request.WithQueryParameter(nameof(query.To), to);
        }

        return SendForUserAsync<IReadOnlyList<EventResponse>>(request, cancellationToken);
    }
}
