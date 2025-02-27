using System.ComponentModel.DataAnnotations;
using Freem.Entities.Identifiers;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Tags.Update;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiUpdateTagRequest = Freem.Web.Api.Public.Contracts.Tags.UpdateTagRequest;
using UseCaseUpdateTagRequest = Freem.Entities.UseCases.Contracts.Tags.Update.UpdateTagRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Tags;

[Authorize]
[Route("api/v1/tags/{tagId}")]
public sealed class UpdateTagController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public UpdateTagController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(
        [Required] [FromRoute] string tagId,
        [Required] [FromBody] ApiUpdateTagRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(tagId, body);

        var response = await _executor.ExecuteAsync<UseCaseUpdateTagRequest, UpdateTagResponse>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static UseCaseUpdateTagRequest Map(string tagIdString, ApiUpdateTagRequest request)
    {
        var tagId = new TagIdentifier(tagIdString);
        return new UseCaseUpdateTagRequest(tagId)
        {
            Name = request.Name?.Map()
        };
    }

    private static IActionResult CreateFailure(Error<UpdateTagErrorCode> error)
    {
        return error.Code switch
        {
            UpdateTagErrorCode.NothingToUpdate => new BadRequestResult(),
            UpdateTagErrorCode.TagNotFound => new NotFoundResult(),
            UpdateTagErrorCode.TagNameAlreadyExists => new UnprocessableEntityResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}