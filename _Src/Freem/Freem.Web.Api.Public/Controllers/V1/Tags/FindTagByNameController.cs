using System.ComponentModel.DataAnnotations;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Models;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Tags.GetByName;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Autherization;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.DTO.Tags;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Tags;

[Authorize(JwtAuthorizationPolicy.Name)]
[Route("api/v1/tags/by-name/{tagName:required}")]
[Tags(ControllerTags.Tags)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TagResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
    [EndpointSummary("Find tags by name")]
    public async Task<IActionResult> GetAsync(
        [Required] [FromRoute] string tagName,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(tagName);

        var response = await _executor.ExecuteAsync<FindTagByNameRequest, FindTagByNameResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(Response, response.Tags, response.TotalCount)
            : CreateFailure(response.Error);
    }

    private static FindTagByNameRequest Map(string tagNameString)
    {
        var tagName = new TagName(tagNameString);
        return new FindTagByNameRequest(tagName);
    }

    private static IActionResult CreateSuccess(
        HttpResponse response, IReadOnlyList<Tag> tag, int totalCount)
    {
        response.Headers.Append(HeaderNames.ItemsCount, tag.Count.ToString());
        response.Headers.Append(HeaderNames.TotalItemsCount, totalCount.ToString());

        var value = MapTags(tag);
        return new OkObjectResult(value);

        static async IAsyncEnumerable<TagResponse> MapTags(IReadOnlyList<Tag> activities)
        {
            foreach (var activity in activities)
                yield return new TagResponse(activity.Id, activity.Name);
        }
    }

    private static IActionResult CreateFailure(Error<FindTagByNameErrorCode> error)
    {
        return error.Code switch
        {
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}