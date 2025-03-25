namespace Freem.Entities.Users.Events;

public static class UserEventActions
{
    public const string Registered = "registered";
    public const string SignedIn = "signed_in";
    public const string SettingsChanged = "settings_changed";
    public const string LoginCredentialsChanged = "password_credentials_changed";
    public const string TelegramIntegrationChanged = "telegram_integration_changed";

    public static IReadOnlyList<string> All { get; } =
    [
        Registered,
        SignedIn,
        SettingsChanged,
        LoginCredentialsChanged,
        TelegramIntegrationChanged,
    ];
}