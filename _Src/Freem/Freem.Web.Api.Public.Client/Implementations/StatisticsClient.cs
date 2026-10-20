using System.Text.Json;
using Freem.Web.Api.Public.Client.Implementations.Base;
using Freem.Web.Api.Public.Contracts.DTO.Statistics;
using Utilities.Http;
using Utilities.Http.Errors.Abstractions;
using Utilities.Primitives;

namespace Freem.Web.Api.Public.Client.Implementations;

public sealed class StatisticsClient : BaseClient
{
    public StatisticsClient(IHttpClient client, JsonSerializerOptions options, ClientSession session) 
        : base(client, options, session)
    {
    }

    public Task<Result<StatisticsPerDaysResponse, IClientError>> GetAsync(
        StatisticsPerDaysRequest query, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get("api/v1/statistics/per-days")
            .WithQueryParameter(nameof(query.Period), query.Period.ToString());

        return SendForUserAsync<StatisticsPerDaysResponse>(request, cancellationToken);
    }
}