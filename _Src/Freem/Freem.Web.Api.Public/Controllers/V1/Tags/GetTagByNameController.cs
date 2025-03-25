using System.ComponentModel.DataAnnotations;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Models;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Tags.GetByName;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.Tags;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Tags;

[Authorize]
[Route("api/v1/tags/by-name/{tagName:required}")]
[Tags(ControllerTags.Tags)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TagResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class GetTagByNameController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public GetTagByNameController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpGet]
    [EndpointSummary("Get tag by name")]
    public async Task<IActionResult> GetAsync(
        [Required] [FromRoute] string tagName,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(tagName);

        var response = await _executor.ExecuteAsync<GetTagByNameRequest, GetTagByNameResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Tag)
            : CreateFailure(response.Error);
    }

    private static GetTagByNameRequest Map(string tagNameString)
    {
        var tagName = new TagName(tagNameString);
        return new GetTagByNameRequest(tagName);
    }

    private static IActionResult CreateSuccess(Tag tag)
    {
        var response = new TagResponse(tag.Id, tag.Name);
        return new OkObjectResult(response);
    }

    private static IActionResult CreateFailure(Error<GetTagByNameErrorCode> error)
    {
        return error.Code switch
        {
            GetTagByNameErrorCode.TagNotFound => new NotFoundResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}