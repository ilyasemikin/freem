using Freem.Entities.UseCases.Contracts.Statistics.PerDays;
using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using ApiStatisticsPerDaysResponse = Freem.Web.Api.Public.Contracts.DTO.Statistics.StatisticsPerDaysResponse;
using ApiStatisticsPerPeriodResponse = Freem.Web.Api.Public.Contracts.DTO.Statistics.StatisticsPerPeriodResponse;

namespace Freem.Web.Api.Public.UseCases.Implementations.Mappers.Responses;

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
