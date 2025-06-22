using System.Text.Json;
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Users.Identifiers;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Http.Requests.Entities.Extensions;
using Freem.Web.Api.Public.Client.Implementations.Base;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.DTO.Events;

namespace Freem.Web.Api.Public.Client.Implementations;

public sealed class EventsClient : BaseClient
{
    public EventsClient(IHttpClient client, JsonSerializerOptions options, TokenLoader tokenLoader) 
        : base(client, options, tokenLoader)
    {
    }

    public Task<ClientResult<IAsyncEnumerable<IEntityEvent<IEntityIdentifier, UserIdentifier>>>> ListAsync(
        ListEventRequest query, 
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("api/v1/events")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameterIfNotNull(nameof(query.After), query.After?.ToString("O"));

        return SendAuthorizedAsync<IAsyncEnumerable<IEntityEvent<IEntityIdentifier, UserIdentifier>>>(
            request, 
            cancellationToken);
    }
}