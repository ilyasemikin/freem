using System.ComponentModel.DataAnnotations;
using Freem.Entities.Identifiers;
using Freem.Entities.Tags;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Tags.Get;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Contracts.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Tags;

[Authorize]
[Route("api/v1/tags/by-name/{tagId}")]
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

    public async Task<ActionResult<TagResponse>> GetAsync(
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

    private static TagResponse CreateSuccess(Tag tag)
    {
        return new TagResponse(tag.Id, tag.Name);
    }

    private static TagResponse CreateFailure(Error<GetTagErrorCode> error)
    {
        throw new NotImplementedException();
    }
}