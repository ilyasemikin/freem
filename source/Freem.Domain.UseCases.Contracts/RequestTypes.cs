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

    public static class DataTransfer
    {
        public static class Export
        {
            public static readonly RequestType Data = RequestType.Parse("data-transfer.exports.data", null);
        }

        public static class Formats
        {
            public static readonly RequestType List = RequestType.Parse("data-transfer.formats.list", null);
        }

        public static class Import
        {
            public static readonly RequestType Apply = RequestType.Parse("data-transfer.imports.apply", null);
            public static readonly RequestType Get = RequestType.Parse("data-transfer.imports.get", null);
            public static readonly RequestType List = RequestType.Parse("data-transfer.imports.list", null);
            public static readonly RequestType ListActivities = RequestType.Parse("data-transfer.imports.list-activities", null);
            public static readonly RequestType ListRows = RequestType.Parse("data-transfer.imports.list-rows", null);
            public static readonly RequestType ListTags = RequestType.Parse("data-transfer.imports.list-tags", null);
            public static readonly RequestType Remove = RequestType.Parse("data-transfer.imports.remove", null);
            public static readonly RequestType UpdateActivityMapping = RequestType.Parse("data-transfer.imports.update-activity-mapping", null);
            public static readonly RequestType UpdateTagMapping = RequestType.Parse("data-transfer.imports.update-tag-mapping", null);
            public static readonly RequestType Upload = RequestType.Parse("data-transfer.imports.upload", null);
        }
    }

    public static class Events
    {
        public static readonly RequestType Get = RequestType.Parse("events.get", null);
        public static readonly RequestType List = RequestType.Parse("events.list", null);
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
