namespace Freem.Domain.Users.Events;

public static class UserEventActions
{
    public const string Registered = "registered";
    public const string SettingsChanged = "settings_changed";

    public static IReadOnlyList<string> All { get; } =
    [
        Registered,
        SettingsChanged,
    ];
}