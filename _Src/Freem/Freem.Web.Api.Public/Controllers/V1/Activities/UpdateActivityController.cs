using System.ComponentModel.DataAnnotations;
using Freem.Entities.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts;
using Freem.Entities.UseCases.Contracts.Activities.Update;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiUpdateActivityRequest = Freem.Web.Api.Public.Contracts.Activities.UpdateActivityRequest;
using UseCaseUpdateActivityRequest = Freem.Entities.UseCases.Contracts.Activities.Update.UpdateActivityRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Activities;

[Authorize]
[Route("api/v1/activities/{activityId}")]
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
    public async Task<ActionResult> UpdateAsync(
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

    private static ActionResult CreateFailure(Error<UpdateActivityErrorCode> error)
    {
        throw new NotImplementedException();
    }
}