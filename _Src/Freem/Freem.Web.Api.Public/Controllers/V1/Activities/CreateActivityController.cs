using System.ComponentModel.DataAnnotations;
using Freem.Entities.Activities;
using Freem.Entities.Relations.Collections;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Activities.Create;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiCreateActivityRequest = Freem.Web.Api.Public.Contracts.Activities.CreateActivityRequest;
using ApiCreateActivityResponse = Freem.Web.Api.Public.Contracts.Activities.CreateActivityResponse;
using UseCaseCreateActivityRequest = Freem.Entities.UseCases.Contracts.Activities.Create.CreateActivityRequest;
using UseCaseCreateActivityResponse = Freem.Entities.UseCases.Contracts.Activities.Create.CreateActivityResponse;

namespace Freem.Web.Api.Public.Controllers.V1.Activities;

[Authorize]
[Route("api/v1/activities")]
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
    public async Task<ActionResult<ApiCreateActivityResponse>> CreateAsync(
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

    private static ApiCreateActivityResponse CreateSuccess(Activity activity)
    {
        return new ApiCreateActivityResponse(activity.Id);
    }

    private static ActionResult<ApiCreateActivityResponse> CreateFailure(Error<CreateActivityErrorCode> error)
    {
        throw new NotImplementedException();
    }
}