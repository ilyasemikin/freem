namespace Freem.Domain.Events;

public static class EventTypes
{
    public static class Activities
    {
        public static readonly EventType Created = EventType.Parse("activities.created", null);
        public static readonly EventType Updated = EventType.Parse("activities.updated", null);
        public static readonly EventType Removed = EventType.Parse("activities.removed", null);
        public static readonly EventType Archived = EventType.Parse("activities.archived", null);
        public static readonly EventType Unarchived = EventType.Parse("activities.unarchived", null);
    }

    public static class Records
    {
        public static readonly EventType Created = EventType.Parse("records.created", null);
        public static readonly EventType Updated = EventType.Parse("records.updated", null);
        public static readonly EventType Removed = EventType.Parse("records.removed", null);
    }

    public static class RunningRecords
    {
        public static readonly EventType Started = EventType.Parse("running_records.started", null);
        public static readonly EventType Stopped = EventType.Parse("running_records.stopped", null);
        public static readonly EventType Updated = EventType.Parse("running_records.updated", null);
        public static readonly EventType Removed = EventType.Parse("running_records.removed", null);
    }

    public static class Tags
    {
        public static readonly EventType Created = EventType.Parse("tags.created", null);
        public static readonly EventType Updated = EventType.Parse("tags.updated", null);
        public static readonly EventType Removed = EventType.Parse("tags.removed", null);
    }

    public static class Users
    {
        public static readonly EventType Registered = EventType.Parse("users.registered", null);
        public static readonly EventType SettingsChanged = EventType.Parse("users.settings_changed", null);
    }
}
