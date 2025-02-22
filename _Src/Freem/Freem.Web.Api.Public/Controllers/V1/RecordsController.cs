using System.ComponentModel.DataAnnotations;
using Freem.Web.Api.Public.Contracts.Records;
using Freem.Web.Api.Public.Contracts.Records.Running;
using Freem.Web.Api.Public.Controllers.V1.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1;

[Route("api/v1/records")]
public sealed class RecordsController : BaseController
{
    [Authorize]
    [HttpPost]
    public Task<ActionResult<CreateRecordResponse>> CreateAsync(
        [Required] [FromBody] CreateRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPut("{recordId}")]
    public Task<ActionResult> UpdateAsync(
        [Required] [FromRoute] string recordId,
        [Required] [FromBody] UpdateRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete("{recordId}")]
    public Task<ActionResult> RemoveAsync(
        [Required] [FromRoute] string recordId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
    [Authorize]
    [HttpGet("{recordId}")]
    public Task<ActionResult<RecordResponse>> GetAsync(
        [Required] [FromRoute] string recordId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet]
    public Task<ActionResult<IAsyncEnumerable<RecordResponse>>> ListAsync(
        [Required] [FromQuery] ListRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet("by-period")]
    public Task<ActionResult<IAsyncEnumerable<RecordResponse>>> ListByPeriodAsync(
        [Required] [FromQuery] ListByPeriodRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost("running/start")]
    public Task<ActionResult> StartRunningAsync(
        [Required] [FromBody] StartRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost("running/stop")]
    public Task<ActionResult> StopRunningAsync(
        [Required] [FromBody] StopRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPut("running")]
    public Task<ActionResult> UpdateRunningAsync(
        [Required] [FromBody] UpdateRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete("running")]
    public Task<ActionResult> RemoveRunningAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet("running")]
    public Task<ActionResult<RunningRecordResponse>> GetRunningAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}