using Freem.Domain.Entities.Users;
using Freem.Domain.UseCases.Context;
using Freem.Domain.Users.Models;
using Freem.Domain.UseCases.Contracts;
using Freem.Domain.UseCases.Contracts.Users.Settings.Get;
using Freem.Domain.UseCases.Contracts.Users.Settings.Update;
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

    public async Task UpdateTimeZoneOffsetAsync(HandlerContext context, TimeZoneOffset offset)
    {
        var request = new UpdateUserSettingsRequest
        {
            TimeZoneOffset = new UpdateField<TimeZoneOffset>(offset)
        };
        var result = await _mediator.ExecuteAsync<UpdateUserSettingsRequest, Result<Error>>(context, request);
        result.EnsureSuccess();
    }

    public async Task UpdateWeekStartAsync(HandlerContext context, WeekStart weekStart)
    {
        var request = new UpdateUserSettingsRequest
        {
            WeekStart = new UpdateField<WeekStart>(weekStart)
        };
        var result = await _mediator.ExecuteAsync<UpdateUserSettingsRequest, Result<Error>>(context, request);
        result.EnsureSuccess();
    }
}