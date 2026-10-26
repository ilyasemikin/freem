using System.Globalization;
using System.Text.Json;
using Freem.Api.Public.Client.Implementations.Base;
using Freem.Api.Public.Contracts.V1.Statistics;
using Freem.Api.Public.Contracts.V1.Statistics.Periods;
using Freem.Api.Public.Contracts.V1.Statistics.Periods.Abstractions;
using Utilities.Http;
using Utilities.Http.Errors.Abstractions;
using Utilities.Primitives;

namespace Freem.Api.Public.Client.Implementations;

public sealed class StatisticsClient : BaseClient
{
    public StatisticsClient(IHttpClient client, JsonSerializerOptions options, ClientSession session) 
        : base(client, options, session)
    {
    }

    public Task<Result<StatisticsPerPeriodResponse, IClientError>> GetPerPeriodAsync(
        StatisticsPerPeriodRequest query, CancellationToken cancellationToken = default)
    {
        var (name, value) = query.Period switch
        {
            DayUnitPeriod day => (nameof(DayUnitPeriod.Day), day.Day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
            MonthUnitPeriod month => (nameof(MonthUnitPeriod.Month), month.Month.ToString()),
            YearUnitPeriod year => (nameof(YearUnitPeriod.Year), year.Year.ToString(CultureInfo.InvariantCulture)),
            _ => throw new InvalidOperationException()
        };

        var unit = query.Period.Unit.ToString();
        var request = HttpRequest.Get("api/v1/statistics/per-period")
            .WithQueryParameter(nameof(IUnitPeriod.Unit), unit)
            .WithQueryParameter(name, value);

        return SendForUserAsync<StatisticsPerPeriodResponse>(request, cancellationToken);
    }
}
