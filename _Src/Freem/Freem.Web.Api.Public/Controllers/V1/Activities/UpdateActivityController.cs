﻿using System.ComponentModel.DataAnnotations;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts;
using Freem.Entities.UseCases.Contracts.Activities.Update;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Mappers;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiUpdateActivityRequest = Freem.Web.Api.Public.Contracts.Activities.UpdateActivityRequest;
using UseCaseUpdateActivityRequest = Freem.Entities.UseCases.Contracts.Activities.Update.UpdateActivityRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Activities;

[Authorize]
[Route("api/v1/activities/{activityId:required}")][ProducesResponseType(StatusCodes.Status200OK)]
[Tags(ControllerTags.Activities)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class UpdateActivityController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public UpdateActivityController(
        UseCaseContextProvider contextProvider,
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpPut]
    [EndpointSummary("Update activity by id")]
    public async Task<IActionResult> UpdateAsync(
        [Required] [FromRoute] string activityId,
        [Required] [FromBody] ApiUpdateActivityRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(activityId, body);
        
        var response = await _executor.ExecuteAsync<UseCaseUpdateActivityRequest, UpdateActivityResponse>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static UseCaseUpdateActivityRequest Map(string activityIdString, ApiUpdateActivityRequest request)
    {
        var activityId = new ActivityIdentifier(activityIdString);

        var tags = request.Tags is not null
            ? new UpdateField<RelatedTagsCollection>(new RelatedTagsCollection(request.Tags.Value))
            : null;
        
        return new UseCaseUpdateActivityRequest(activityId)
        {
            Name = request.Name?.Map(),
            Tags = tags
        };
    }

    private static IActionResult CreateFailure(Error<UpdateActivityErrorCode> error)
    {
        return error.Code switch
        {
            UpdateActivityErrorCode.ActivityNotFound => new NotFoundResult(),
            UpdateActivityErrorCode.RelatedTagsNotFound => new UnprocessableEntityResult(),
            UpdateActivityErrorCode.NothingToUpdate => new BadRequestResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}