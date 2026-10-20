using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Tags;
using Freem.Entities.UseCases.Context;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Mappers;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;
using ApiUpdateTagRequest = Freem.Web.Api.Public.Contracts.DTO.Tags.UpdateTagRequest;
using UseCaseUpdateTagRequest = Freem.Entities.UseCases.Contracts.Tags.Update.UpdateTagRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Tags;

[Route("api/v1/tags/{tagId:required}")]
[Tags(ControllerTags.Tags)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class UpdateTagController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public UpdateTagController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpPut]
    [EndpointSummary("Update tag by id")]
    public async Task<IActionResult> UpdateAsync(
        [Required] [FromRoute] string tagId,
        [Required] [FromBody] ApiUpdateTagRequest body,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(tagId, body);

        var response = await _mediator.ExecuteAsync<UseCaseUpdateTagRequest, Result<Error>>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static UseCaseUpdateTagRequest Map(string tagIdString, ApiUpdateTagRequest request)
    {
        var tagId = TagId.Parse(tagIdString, null);
        return new UseCaseUpdateTagRequest(tagId)
        {
            Name = request.Name?.Map()
        };
    }

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.NothingToDo)
            return new BadRequestResult();
        if (error.Type == DomainErrorCatalog.Types.EntityNotFound)
            return new NotFoundResult();
        if (error.Type == DomainErrorCatalog.Types.TagNameAlreadyExists)
            return new UnprocessableEntityResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}