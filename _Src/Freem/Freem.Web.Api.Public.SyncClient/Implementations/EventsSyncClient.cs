using System.Text.Json;
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Users.Identifiers;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Web.Api.Public.Client;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.DTO.Events;
using Freem.Web.Api.Public.SyncClient.Implementations.Base;

namespace Freem.Web.Api.Public.SyncClient.Implementations;

public sealed class EventsSyncClient : BaseSyncClient
{
    public EventsSyncClient(IHttpClient client, JsonSerializerOptions options, TokenLoader tokenLoader) 
        : base(client, options, tokenLoader)
    {
    }

    public ClientResult<IAsyncEnumerable<IEntityEvent<IEntityIdentifier, UserIdentifier>>> List(ListEventRequest query)
    {
        var request = HttpRequest.Get("api/v1/events")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString());

        return SendAuthorized<IAsyncEnumerable<IEntityEvent<IEntityIdentifier, UserIdentifier>>>(request);
    }
}