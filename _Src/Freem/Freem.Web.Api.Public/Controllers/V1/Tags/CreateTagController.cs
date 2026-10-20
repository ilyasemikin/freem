using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Entities.Tags;
using Freem.Entities.UseCases.Context;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;
using ApiCreateTagRequest = Freem.Web.Api.Public.Contracts.DTO.Tags.CreateTagRequest;
using ApiCreateTagResponse = Freem.Web.Api.Public.Contracts.DTO.Tags.CreateTagResponse;
using UseCaseCreateTagRequest = Freem.Entities.UseCases.Contracts.Tags.Create.CreateTagRequest;
using UseCaseCreateTagResponse = Freem.Entities.UseCases.Contracts.Tags.Create.CreateTagResponse;

namespace Freem.Web.Api.Public.Controllers.V1.Tags;

[Route("api/v1/tags")]
[Tags(ControllerTags.Tags)]
[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreateTagResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class CreateTagController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public CreateTagController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpPost]
    [EndpointSummary("Create tag")]
    public async Task<IActionResult> CreateAsync(
        [Required] [FromBody] ApiCreateTagRequest body,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(body);

        var response = await _mediator.ExecuteAsync<UseCaseCreateTagRequest, Result<UseCaseCreateTagResponse, Error>>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Value.Tag)
            : CreateFailure(response.Error);
    }

    private static UseCaseCreateTagRequest Map(ApiCreateTagRequest request)
    {
        return new UseCaseCreateTagRequest(request.Name);
    }

    private static IActionResult CreateSuccess(Tag tag)
    {
        var response = new ApiCreateTagResponse(tag.Id);
        return new CreatedResult()
        {
            Value = response
        };
    }

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.TagNameAlreadyExists)
            return new UnprocessableEntityResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}