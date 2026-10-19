using System.ComponentModel.DataAnnotations;
using Freem.Domain.Tags.Models;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Tags.GetByName;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.DTO.Tags;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Tags;

[Route("api/v1/tags/by-name/{tagName:required}")]
[Tags(ControllerTags.Tags)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TagResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class FindTagByNameController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public FindTagByNameController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpGet]
    [EndpointSummary("Find tag by name")]
    public async Task<IActionResult> GetAsync(
        [Required] [FromRoute] string tagName,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(tagName);

        var response = await _executor.ExecuteAsync<FindTagByNameRequest, FindTagByNameResponse>(context, request, cancellationToken);

        if (!response.Success)
            return CreateFailure(response.Error);

        var tag = response.Tags.FirstOrDefault();
        if (tag is null)
            return new NotFoundResult();

        var value = new TagResponse(tag.Id, tag.Name);
        return new OkObjectResult(value);
    }

    private static FindTagByNameRequest Map(string tagNameString)
    {
        var tagName = new TagName(tagNameString);
        return new FindTagByNameRequest(tagName);
    }

    private static IActionResult CreateFailure(Error<FindTagByNameErrorCode> error)
    {
        return error.Code switch
        {
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}