using Utilities.UseCases.Models;

namespace Freem.Entities.UseCases.Contracts;

public static class RequestTypes
{
    public static class Activities
    {
        public static readonly RequestType Archive = RequestType.Parse("activities.archive", null);
        public static readonly RequestType Create = RequestType.Parse("activities.create", null);
        public static readonly RequestType Find = RequestType.Parse("activities.find", null);
        public static readonly RequestType Get = RequestType.Parse("activities.get", null);
        public static readonly RequestType List = RequestType.Parse("activities.list", null);
        public static readonly RequestType Remove = RequestType.Parse("activities.remove", null);
        public static readonly RequestType Unarchive = RequestType.Parse("activities.unarchive", null);
        public static readonly RequestType Update = RequestType.Parse("activities.update", null);
    }

    public static class Events
    {
        public static readonly RequestType List = RequestType.Parse("events.list", null);
    }

    public static class Records
    {
        public static readonly RequestType Create = RequestType.Parse("records.create", null);
        public static readonly RequestType Get = RequestType.Parse("records.get", null);
        public static readonly RequestType List = RequestType.Parse("records.list", null);
        public static readonly RequestType PeriodList = RequestType.Parse("records.period-list", null);
        public static readonly RequestType Remove = RequestType.Parse("records.remove", null);
        public static readonly RequestType Update = RequestType.Parse("records.update", null);
    }

    public static class RunningRecords
    {
        public static readonly RequestType Get = RequestType.Parse("running-records.get", null);
        public static readonly RequestType Remove = RequestType.Parse("running-records.remove", null);
        public static readonly RequestType Start = RequestType.Parse("running-records.start", null);
        public static readonly RequestType Stop = RequestType.Parse("running-records.stop", null);
        public static readonly RequestType Update = RequestType.Parse("running-records.update", null);
    }

    public static class Statistics
    {
        public static readonly RequestType PerDays = RequestType.Parse("statistics.per-days", null);
        public static readonly RequestType PerPeriod = RequestType.Parse("statistics.per-period", null);
    }

    public static class Tags
    {
        public static readonly RequestType Create = RequestType.Parse("tags.create", null);
        public static readonly RequestType FindByName = RequestType.Parse("tags.find-by-name", null);
        public static readonly RequestType Get = RequestType.Parse("tags.get", null);
        public static readonly RequestType List = RequestType.Parse("tags.list", null);
        public static readonly RequestType Remove = RequestType.Parse("tags.remove", null);
        public static readonly RequestType Update = RequestType.Parse("tags.update", null);
    }

    public static class Users
    {
        public static readonly RequestType Get = RequestType.Parse("users.get", null);
        public static readonly RequestType Register = RequestType.Parse("users.register", null);

        public static class Settings
        {
            public static readonly RequestType Get = RequestType.Parse("users.settings.get", null);
            public static readonly RequestType Update = RequestType.Parse("users.settings.update", null);
        }
    }
}
