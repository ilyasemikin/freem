using System.Text.Json;
using Freem.Domain.Events;
using Freem.Domain.Identifiers;
using Freem.Domain.Users.Identifiers;
using Freem.Web.Api.Public.Client;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.DTO.Events;
using Freem.Web.Api.Public.SyncClient.Implementations.Base;
using Utilities.Http;

namespace Freem.Web.Api.Public.SyncClient.Implementations;

public sealed class EventsSyncClient : BaseSyncClient
{
    public EventsSyncClient(IHttpClient client, JsonSerializerOptions options, ClientSession session) 
        : base(client, options, session)
    {
    }

    public ClientResult<IAsyncEnumerable<IEntityEvent<IEntityIdentifier, UserIdentifier>>> List(ListEventRequest query)
    {
        var request = HttpRequest.Get("api/v1/events")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString());

        return SendForUser<IAsyncEnumerable<IEntityEvent<IEntityIdentifier, UserIdentifier>>>(request);
    }
}