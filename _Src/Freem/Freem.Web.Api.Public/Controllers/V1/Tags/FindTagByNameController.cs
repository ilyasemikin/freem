using System.ComponentModel.DataAnnotations;
using Freem.Domain.Tags.Models;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Tags.GetByName;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.DTO.Tags;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers.V1.Tags;

[Route("api/v1/tags/by-name/{tagName:required}")]
[Tags(ControllerTags.Tags)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TagResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class FindTagByNameController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public FindTagByNameController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpGet]
    [EndpointSummary("Find tag by name")]
    public async Task<IActionResult> GetAsync(
        [Required] [FromRoute] string tagName,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(tagName);

        var response = await _mediator.ExecuteAsync<FindTagByNameRequest, Result<FindTagByNameResponse, Error>>(context, request, cancellationToken);

        if (!response.Success)
            return CreateFailure(response.Error);

        var tag = response.Value.Tags.FirstOrDefault();
        if (tag is null)
            return new NotFoundResult();

        var value = new TagResponse(tag.Id, tag.Name);
        return new OkObjectResult(value);
    }

    private static FindTagByNameRequest Map(string tagNameString)
    {
        var tagName = TagName.Parse(tagNameString, null);
        var searchText = tagName.ToString();
        return new FindTagByNameRequest(searchText);
    }

    private static IActionResult CreateFailure(Error error)
    {
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}