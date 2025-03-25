using System.Text.Json;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Web.Api.Public.Client;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.Statistics;
using Freem.Web.Api.Public.SyncClient.Implementations.Base;

namespace Freem.Web.Api.Public.SyncClient.Implementations;

public sealed class StatisticsSyncClient : BaseSyncClient
{
    public StatisticsSyncClient(IHttpClient client, JsonSerializerOptions options, TokenLoader tokenLoader) 
        : base(client, options, tokenLoader)
    {
    }

    public ClientResult<StatisticsPerDaysResponse> Get(StatisticsPerDaysRequest query)
    {
        var request = HttpRequest.Get("api/v1/statistics/per-days")
            .WithQueryParameter(nameof(query.Period), query.Period.ToString());
        
        return SendAuthorized<StatisticsPerDaysResponse>(request);
    }
}