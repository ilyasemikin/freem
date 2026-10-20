using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Records;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Records.Remove;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers.V1.Records;

[Route("api/v1/records/{recordId:required}")]
[Tags(ControllerTags.Records)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class RemoveRecordController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;
    
    public RemoveRecordController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }
    
    [HttpDelete]
    [EndpointSummary("Remove record by id")]
    public async Task<IActionResult> RemoveAsync(
        [Required] [FromRoute] string recordId,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(recordId);

        var response = await _mediator.ExecuteAsync<RemoveRecordRequest, Result<Error>>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static RemoveRecordRequest Map(string recordIdString)
    {
        var recordId = RecordId.Parse(recordIdString, null);
        return new RemoveRecordRequest(recordId);
    }

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.EntityNotFound)
            return new NotFoundResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}