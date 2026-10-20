using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Tags;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Tags.Remove;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers.V1.Tags;

[Route("api/v1/tags/{tagId:required}")]
[Tags(ControllerTags.Tags)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class RemoveTagController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public RemoveTagController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpDelete]
    [EndpointSummary("Remove tag by id")]
    public async Task<IActionResult> RemoveAsync(
        [Required] [FromRoute] string tagId,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(tagId);

        var response = await _mediator.ExecuteAsync<RemoveTagRequest, Result<Error>>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static RemoveTagRequest Map(string tagIdString)
    {
        var tagId = TagId.Parse(tagIdString, null);
        return new RemoveTagRequest(tagId);
    }

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.EntityNotFound)
            return new NotFoundResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}