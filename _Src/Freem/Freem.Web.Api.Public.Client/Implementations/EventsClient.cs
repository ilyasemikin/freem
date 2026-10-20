using System.Text.Json;
using Freem.Domain;
using Freem.Domain.Events;
using Freem.Domain.Users;
using Freem.Web.Api.Public.Client.Implementations.Base;
using Freem.Web.Api.Public.Contracts.DTO.Events;
using Utilities.Http;
using Utilities.Http.Errors.Abstractions;
using Utilities.Primitives;

namespace Freem.Web.Api.Public.Client.Implementations;

public sealed class EventsClient : BaseClient
{
    public EventsClient(IHttpClient client, JsonSerializerOptions options, ClientSession session) 
        : base(client, options, session)
    {
    }

    public Task<Result<IReadOnlyList<IEntityEvent<Id, UserId>>, IClientError>> ListAsync(
        ListEventRequest query, 
        CancellationToken cancellationToken = default)
    {
        var limit = query.Limit.ToString();
        var request = HttpRequest.Get("api/v1/events")
            .WithQueryParameter(nameof(query.Limit), limit);

        if (query.After is not null)
        {
            var after = query.After.Value.ToString("O");
            request = request.WithQueryParameter(nameof(query.After), after);
        }

        return SendForUserAsync<IReadOnlyList<IEntityEvent<Id, UserId>>>(
            request, 
            cancellationToken);
    }
}