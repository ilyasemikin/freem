using System.ComponentModel.DataAnnotations;
using Freem.Entities.Identifiers;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Activities.Unarchive;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Activities;

[Authorize]
[Route("api/v1/activities/{activityId}/unarchive")]
public sealed class UnarchiveActivityController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public UnarchiveActivityController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpPost]
    public async Task<ActionResult> UnarchiveAsync(
        [Required] [FromRoute] string activityId,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(activityId);

        var response = await _executor.ExecuteAsync<UnarchiveActivityRequest, UnarchiveActivityResponse>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static UnarchiveActivityRequest Map(string activityIdString)
    {
        var activityId = new ActivityIdentifier(activityIdString);
        return new UnarchiveActivityRequest(activityId);
    }

    private static ActionResult CreateFailure(Error<UnarchiveActivityErrorCode> error)
    {
        throw new NotImplementedException();
    }
}