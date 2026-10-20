using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Entities.Tags;
using Freem.Domain.Tags;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Tags.Get;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.DTO.Tags;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers.V1.Tags;

[Route("api/v1/tags/{tagId:required}")]
[Tags(ControllerTags.Tags)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetTagResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class GetTagController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public GetTagController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpGet]
    [EndpointSummary("Get tag by id")]
    public async Task<IActionResult> GetAsync(
        [Required] [FromRoute] string tagId,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(tagId);

        var response = await _mediator.ExecuteAsync<GetTagRequest, Result<GetTagResponse, Error>>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Value.Tag)
            : CreateFailure(response.Error);
    }

    private static GetTagRequest Map(string tagIdString)
    {
        var tagId = TagId.Parse(tagIdString, null);
        return new GetTagRequest(tagId);
    }

    private static IActionResult CreateSuccess(Tag tag)
    {
        var response = new TagResponse(tag.Id, tag.Name);
        return new OkObjectResult(response);
    }

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.EntityNotFound)
            return new NotFoundResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}