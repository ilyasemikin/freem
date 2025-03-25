using System.ComponentModel.DataAnnotations;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Tags.Get;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.Tags;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Tags;

[Authorize]
[Route("api/v1/tags/{tagId:required}")]
[Tags(ControllerTags.Tags)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetTagResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class GetTagController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public GetTagController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpGet]
    [EndpointSummary("Get tag by id")]
    public async Task<IActionResult> GetAsync(
        [Required] [FromRoute] string tagId,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(tagId);

        var response = await _executor.ExecuteAsync<GetTagRequest, GetTagResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Tag)
            : CreateFailure(response.Error);
    }

    private static GetTagRequest Map(string tagIdString)
    {
        var tagId = new TagIdentifier(tagIdString);
        return new GetTagRequest(tagId);
    }

    private static IActionResult CreateSuccess(Tag tag)
    {
        var response = new TagResponse(tag.Id, tag.Name);
        return new OkObjectResult(response);
    }

    private static IActionResult CreateFailure(Error<GetTagErrorCode> error)
    {
        return error.Code switch
        {
            GetTagErrorCode.TagNotFound => new NotFoundResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}