using Freem.Domain.Entities.Users;

namespace Freem.Domain.UseCases.Contracts.Users.Settings.Get;

public sealed class GetUserSettingsResponse
{
    public UserSettings Settings { get; }

    public GetUserSettingsResponse(UserSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        Settings = settings;
    }
}