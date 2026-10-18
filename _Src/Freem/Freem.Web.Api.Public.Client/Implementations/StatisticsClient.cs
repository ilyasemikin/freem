using System.Text.Json;
using Freem.Web.Api.Public.Client.Implementations.Base;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.DTO.Statistics;
using Utilities.Http;

namespace Freem.Web.Api.Public.Client.Implementations;

public sealed class StatisticsClient : BaseClient
{
    public StatisticsClient(IHttpClient client, JsonSerializerOptions options, CurrentUser currentUser) 
        : base(client, options, currentUser)
    {
    }

    public Task<ClientResult<StatisticsPerDaysResponse>> GetAsync(
        StatisticsPerDaysRequest query, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("api/v1/statistics/per-days")
            .WithQueryParameter(nameof(query.Period), query.Period.ToString());

        return SendForUserAsync<StatisticsPerDaysResponse>(request, cancellationToken);
    }
}