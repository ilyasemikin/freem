using Freem.Domain.Entities.Users;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Users.Settings.Get;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Users.Settings;

internal static class UserSettingsRequestMediatorExtensions
{
    public static async Task<Result<UserSettings, Error>> GetUserSettingsAsync(
        this IRequestMediator<HandlerContext> mediator,
        HandlerContext context,
        CancellationToken cancellationToken)
    {
        var request = GetUserSettingsRequest.Instance;
        var result = await mediator.ExecuteAsync<GetUserSettingsRequest, Result<GetUserSettingsResponse, Error>>(context, request, cancellationToken);
        if (!result.Success)
            return result.Error;

        return result.Value.Settings;
    }
}
