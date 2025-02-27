using System.ComponentModel.DataAnnotations;
using Freem.Entities.Tags;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Tags.Create;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiCreateTagRequest = Freem.Web.Api.Public.Contracts.Tags.CreateTagRequest;
using ApiCreateTagResponse = Freem.Web.Api.Public.Contracts.Tags.CreateTagResponse;
using UseCaseCreateTagRequest = Freem.Entities.UseCases.Contracts.Tags.Create.CreateTagRequest;
using UseCaseCreateTagResponse = Freem.Entities.UseCases.Contracts.Tags.Create.CreateTagResponse;

namespace Freem.Web.Api.Public.Controllers.V1.Tags;

[Authorize]
[Route("api/v1/tags")]
public sealed class CreateTagController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public CreateTagController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreateTagResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(
        [Required] [FromBody] ApiCreateTagRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(body);

        var response = await _executor.ExecuteAsync<UseCaseCreateTagRequest, UseCaseCreateTagResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Tag)
            : CreateFailure(response.Error);
    }

    private static UseCaseCreateTagRequest Map(ApiCreateTagRequest request)
    {
        return new UseCaseCreateTagRequest(request.Name);
    }

    private static IActionResult CreateSuccess(Tag tag)
    {
        var response = new ApiCreateTagResponse(tag.Id);
        return new CreatedResult()
        {
            Value = response
        };
    }

    private static IActionResult CreateFailure(Error<CreateTagErrorCode> error)
    {
        return error.Code switch
        {
            CreateTagErrorCode.TagNameAlreadyExists => new UnprocessableEntityResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}