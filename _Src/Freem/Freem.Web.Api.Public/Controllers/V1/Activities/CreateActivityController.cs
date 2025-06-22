using System.ComponentModel.DataAnnotations;
using Freem.Entities.Activities;
using Freem.Entities.Relations.Collections;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Activities.Create;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Autherization;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiCreateActivityRequest = Freem.Web.Api.Public.Contracts.DTO.Activities.CreateActivityRequest;
using ApiCreateActivityResponse = Freem.Web.Api.Public.Contracts.DTO.Activities.CreateActivityResponse;
using UseCaseCreateActivityRequest = Freem.Entities.UseCases.Contracts.Activities.Create.CreateActivityRequest;
using UseCaseCreateActivityResponse = Freem.Entities.UseCases.Contracts.Activities.Create.CreateActivityResponse;

namespace Freem.Web.Api.Public.Controllers.V1.Activities;

[Authorize(JwtAuthorizationPolicy.Name)]
[Route("api/v1/activities")]
[Tags(ControllerTags.Activities)]
[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreateActivityResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class CreateActivityController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public CreateActivityController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpPost]
    [EndpointSummary("Create activity")]
    public async Task<IActionResult> CreateAsync(
        [Required] [FromBody] ApiCreateActivityRequest body, 
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(body);
        
        var response = await _executor.ExecuteAsync<UseCaseCreateActivityRequest, UseCaseCreateActivityResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Activity)
            : CreateFailure(response.Error);
    }

    private static UseCaseCreateActivityRequest Map(ApiCreateActivityRequest request)
    {
        return new UseCaseCreateActivityRequest(request.Name)
        {
            Tags = new RelatedTagsCollection(request.Tags)
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

    private static IActionResult CreateFailure(Error<CreateActivityErrorCode> error)
    {
        return error.Code switch
        {
            CreateActivityErrorCode.RelatedTagsNotFound => new UnprocessableEntityResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}