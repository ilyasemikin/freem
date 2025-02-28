using System.ComponentModel.DataAnnotations;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Tags.Remove;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Tags;

[Authorize]
[Route("api/v1/tags/{tagId}")]
public sealed class RemoveTagController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public RemoveTagController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync(
        [Required] [FromRoute] string tagId,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(tagId);

        var response = await _executor.ExecuteAsync<RemoveTagRequest, RemoveTagResponse>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static RemoveTagRequest Map(string tagIdString)
    {
        var tagId = new TagIdentifier(tagIdString);
        return new RemoveTagRequest(tagId);
    }

    private static IActionResult CreateFailure(Error<RemoveTagErrorCode> error)
    {
        return error.Code switch
        {
            RemoveTagErrorCode.TagNotFound => new NotFoundResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}