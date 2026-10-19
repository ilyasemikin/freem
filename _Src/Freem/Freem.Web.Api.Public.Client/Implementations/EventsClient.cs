using System.Text.Json;
using Freem.Domain.Events;
using Freem.Domain.Identifiers;
using Freem.Domain.Users.Identifiers;
using Freem.Web.Api.Public.Client.Implementations.Base;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.DTO.Events;
using Utilities.Http;

namespace Freem.Web.Api.Public.Client.Implementations;

public sealed class EventsClient : BaseClient
{
    public EventsClient(IHttpClient client, JsonSerializerOptions options, ClientSession session) 
        : base(client, options, session)
    {
    }

    public Task<ClientResult<IAsyncEnumerable<IEntityEvent<IEntityIdentifier, UserIdentifier>>>> ListAsync(
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

        return SendForUserAsync<IAsyncEnumerable<IEntityEvent<IEntityIdentifier, UserIdentifier>>>(
            request, 
            cancellationToken);
    }
}