using System.Text.Json;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Web.Api.Public.Client.Implementations.Base;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.DTO.Statistics;

namespace Freem.Web.Api.Public.Client.Implementations;

public sealed class StatisticsClient : BaseClient
{
    public StatisticsClient(IHttpClient client, JsonSerializerOptions options, TokenLoader tokenLoader) 
        : base(client, options, tokenLoader)
    {
    }

    public Task<ClientResult<StatisticsPerDaysResponse>> GetAsync(
        StatisticsPerDaysRequest query, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("api/v1/statistics/per-days")
            .WithQueryParameter(nameof(query.Period), query.Period.ToString());

        return SendAuthorizedAsync<StatisticsPerDaysResponse>(request, cancellationToken);
    }
}