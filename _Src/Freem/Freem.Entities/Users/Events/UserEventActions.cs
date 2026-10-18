namespace Freem.Entities.Users.Events;

public static class UserEventActions
{
    public const string Registered = "registered";
    public const string SettingsChanged = "settings_changed";
    public const string TelegramIntegrationChanged = "telegram_integration_changed";

    public static IReadOnlyList<string> All { get; } =
    [
        Registered,
        SettingsChanged,
        TelegramIntegrationChanged,
    ];
}