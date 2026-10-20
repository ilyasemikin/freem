using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Entities.Activities;
using Freem.Domain.Relations.Collections;
using Freem.Entities.UseCases.Context;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;
using ApiCreateActivityRequest = Freem.Web.Api.Public.Contracts.DTO.Activities.CreateActivityRequest;
using ApiCreateActivityResponse = Freem.Web.Api.Public.Contracts.DTO.Activities.CreateActivityResponse;
using UseCaseCreateActivityRequest = Freem.Entities.UseCases.Contracts.Activities.Create.CreateActivityRequest;
using UseCaseCreateActivityResponse = Freem.Entities.UseCases.Contracts.Activities.Create.CreateActivityResponse;

namespace Freem.Web.Api.Public.Controllers.V1.Activities;

[Route("api/v1/activities")]
[Tags(ControllerTags.Activities)]
[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreateActivityResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class CreateActivityController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public CreateActivityController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpPost]
    [EndpointSummary("Create activity")]
    public async Task<IActionResult> CreateAsync(
        [Required] [FromBody] ApiCreateActivityRequest body, 
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(body);
        
        var response = await _mediator.ExecuteAsync<UseCaseCreateActivityRequest, Result<UseCaseCreateActivityResponse, Error>>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Value.Activity)
            : CreateFailure(response.Error);
    }

    private static UseCaseCreateActivityRequest Map(ApiCreateActivityRequest request)
    {
        return new UseCaseCreateActivityRequest(request.Name)
        {
            Tags = new RelatedTagIdsCollection(request.Tags)
        };
    }

    private static IActionResult CreateSuccess(Activity activity)
    {
        var response = new ApiCreateActivityResponse(activity.Id);
        return new CreatedResult
        {
            Value = response
        };
    }

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.RelatedEntitiesNotFound)
            return new UnprocessableEntityResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}