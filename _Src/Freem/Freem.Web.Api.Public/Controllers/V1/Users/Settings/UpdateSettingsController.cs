using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Entities.UseCases.Context;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Mappers;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;
using ApiUpdateUserSettingsRequest = Freem.Web.Api.Public.Contracts.DTO.Users.Settings.UpdateUserSettingsRequest;
using UseCaseUpdateUserSettingsRequest = Freem.Entities.UseCases.Contracts.Users.Settings.Update.UpdateUserSettingsRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Users.Settings;

[Route("api/v1/user/settings")]
[Tags(ControllerTags.User, ControllerTags.Settings)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class UpdateSettingsController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public UpdateSettingsController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpPut]
    [EndpointSummary("Update user settings")]
    public async Task<IActionResult> UpdateAsync(
        [Required] [FromBody] ApiUpdateUserSettingsRequest body,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(body);

        var response = await _mediator.ExecuteAsync<UseCaseUpdateUserSettingsRequest, Result<Error>>(context, request, cancellationToken);

        return response.Success
            ? Ok()
            : CreateFailure(response.Error);
    }

    private static UseCaseUpdateUserSettingsRequest Map(ApiUpdateUserSettingsRequest request)
    {
        return new UseCaseUpdateUserSettingsRequest()
        {
            UtcOffset = request.DayUtcOffset?.Map()
        };
    }

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.EntityNotFound)
            return new ForbidResult();
        if (error.Type == DomainErrorCatalog.Types.NothingToDo)
            return new BadRequestResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}