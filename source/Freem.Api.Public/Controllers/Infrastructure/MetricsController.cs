using Freem.Api.Public.UseCases.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Metrics.Prometheus.Abstractions;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.Infrastructure;

/// <summary>Service metrics in the Prometheus exposition format. Anonymous.</summary>
[AllowAnonymous]
[Route("_metrics")]
public sealed class MetricsController : BaseController
{
    private readonly IMetricsScraper _scraper;

    public MetricsController(IRequestMediator<ApiAuthorizedHandlerContext> mediator, IMetricsScraper scraper)
        : base(mediator)
    {
        ArgumentNullException.ThrowIfNull(scraper);

        _scraper = scraper;
    }

    /// <summary>Returns the current metrics; the format (Prometheus text or OpenMetrics) is chosen from the Accept header.</summary>
    [HttpGet]
    [Produces("text/plain", "application/openmetrics-text")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task GetMetrics()
    {
        return _scraper.ScrapeAsync(HttpContext);
    }
}
