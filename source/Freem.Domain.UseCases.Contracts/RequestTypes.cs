using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts;

public static class RequestTypes
{
    public static class Activities
    {
        public static readonly RequestType Archive = RequestType.Parse("activities.archive", null);
        public static readonly RequestType Create = RequestType.Parse("activities.create", null);
        public static readonly RequestType Get = RequestType.Parse("activities.get", null);
        public static readonly RequestType List = RequestType.Parse("activities.list", null);
        public static readonly RequestType Remove = RequestType.Parse("activities.remove", null);
        public static readonly RequestType Search = RequestType.Parse("activities.search", null);
        public static readonly RequestType Unarchive = RequestType.Parse("activities.unarchive", null);
        public static readonly RequestType Update = RequestType.Parse("activities.update", null);
    }

    public static class Events
    {
        public static readonly RequestType Get = RequestType.Parse("events.get", null);
        public static readonly RequestType List = RequestType.Parse("events.list", null);
    }

    public static class Export
    {
        public static readonly RequestType Data = RequestType.Parse("export.data", null);
    }

    public static class Formats
    {
        public static readonly RequestType List = RequestType.Parse("formats.list", null);
    }

    public static class Imports
    {
        public static readonly RequestType Apply = RequestType.Parse("imports.apply", null);
        public static readonly RequestType Get = RequestType.Parse("imports.get", null);
        public static readonly RequestType List = RequestType.Parse("imports.list", null);
        public static readonly RequestType ListActivities = RequestType.Parse("imports.list-activities", null);
        public static readonly RequestType ListRows = RequestType.Parse("imports.list-rows", null);
        public static readonly RequestType ListTags = RequestType.Parse("imports.list-tags", null);
        public static readonly RequestType Remove = RequestType.Parse("imports.remove", null);
        public static readonly RequestType UpdateActivityMapping = RequestType.Parse("imports.update-activity-mapping", null);
        public static readonly RequestType UpdateTagMapping = RequestType.Parse("imports.update-tag-mapping", null);
        public static readonly RequestType Upload = RequestType.Parse("imports.upload", null);
    }

    public static class Records
    {
        public static readonly RequestType AddPeriod = RequestType.Parse("records.add-period", null);
        public static readonly RequestType Create = RequestType.Parse("records.create", null);
        public static readonly RequestType Get = RequestType.Parse("records.get", null);
        public static readonly RequestType List = RequestType.Parse("records.list", null);
        public static readonly RequestType ListByPeriod = RequestType.Parse("records.list-by-period", null);
        public static readonly RequestType Remove = RequestType.Parse("records.remove", null);
        public static readonly RequestType RemovePeriod = RequestType.Parse("records.remove-period", null);
        public static readonly RequestType Update = RequestType.Parse("records.update", null);
        public static readonly RequestType UpdatePeriod = RequestType.Parse("records.update-period", null);
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
        public static readonly RequestType Get = RequestType.Parse("tags.get", null);
        public static readonly RequestType List = RequestType.Parse("tags.list", null);
        public static readonly RequestType Remove = RequestType.Parse("tags.remove", null);
        public static readonly RequestType SearchByName = RequestType.Parse("tags.search-by-name", null);
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
