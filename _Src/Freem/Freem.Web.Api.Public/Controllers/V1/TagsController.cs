using System.ComponentModel.DataAnnotations;
using Freem.Web.Api.Public.Contracts.Tags;
using Freem.Web.Api.Public.Controllers.V1.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1;

[Route("api/v1/tags")]
public sealed class TagsController : BaseController
{
    [Authorize]
    [HttpPost]
    public Task<ActionResult<CreateTagResponse>> CreateAsync(
        [Required] [FromBody] CreateTagRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPut("{tagId}")]
    public Task<ActionResult> UpdateAsync(
        [Required] [FromRoute] string tagId,
        [Required] [FromBody] UpdateTagRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPut("by-name/{tagName}")]
    public Task<ActionResult> UpdateByNameAsync(
        [Required] [FromRoute] string tagName,
        [Required] [FromBody] UpdateTagRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete("{tagId}")]
    public Task<ActionResult> RemoveAsync(
        [Required] [FromRoute] string tagId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete("by-name/{tagName}")]
    public Task<ActionResult> RemoveByNameAsync(
        [Required] [FromRoute] string tagName,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet("{tagId}")]
    public Task<ActionResult<TagResponse>> GetAsync(
        [Required] [FromRoute] string tagId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet("by-name/{tagName}")]
    public Task<ActionResult<TagResponse>> GetByNameAsync(
        [Required] [FromRoute] string tagName,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet]
    public Task<ActionResult<IAsyncEnumerable<TagResponse>>> ListAsync(
        [Required] [FromQuery] ListTagRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}