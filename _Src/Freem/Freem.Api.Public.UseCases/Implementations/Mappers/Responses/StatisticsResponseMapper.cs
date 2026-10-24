using Freem.Domain.UseCases.Contracts.Statistics.PerDays;
using Freem.Domain.UseCases.Contracts.Statistics.PerPeriod;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using ApiStatisticsPerDaysResponse = Freem.Api.Public.Contracts.V1.Statistics.StatisticsPerDaysResponse;
using ApiStatisticsPerPeriodResponse = Freem.Api.Public.Contracts.V1.Statistics.StatisticsPerPeriodResponse;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Responses;

internal static class StatisticsResponseMapper
{
    public static IActionResult Map(this Result<StatisticsPerDaysResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = new ApiStatisticsPerDaysResponse(result.Value.Statistics);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<StatisticsPerPeriodResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = new ApiStatisticsPerPeriodResponse(result.Value.Statistics);
        return new OkObjectResult(response);
    }
}
