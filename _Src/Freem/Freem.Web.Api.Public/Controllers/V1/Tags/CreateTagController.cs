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
    public async Task<ActionResult<ApiCreateTagResponse>> CreateAsync(
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

    private static ApiCreateTagResponse CreateSuccess(Tag tag)
    {
        return new ApiCreateTagResponse(tag.Id);
    }

    private static ActionResult<ApiCreateTagResponse> CreateFailure(Error<CreateTagErrorCode> error)
    {
        throw new NotImplementedException();
    }
}