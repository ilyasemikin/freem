using Freem.Domain.Entities.Users;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Users.Settings.Get;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.IntegrationTests.Fixtures.Executors;

public sealed class UsersSettingsExecutor
{
    private readonly IRequestMediator<HandlerContext> _mediator;

    public UsersSettingsExecutor(IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        _mediator = mediator;
    }

    public async Task<UserSettings> RequiredGetAsync(HandlerContext context)
    {
        var request = GetUserSettingsRequest.Instance;
        var result = await _mediator.ExecuteAsync<GetUserSettingsRequest, Result<GetUserSettingsResponse, Error>>(context, request);
        var response = result.EnsureSuccess();
        return response.Settings;
    }
}