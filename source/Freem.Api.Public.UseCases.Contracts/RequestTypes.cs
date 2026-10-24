using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts;

public static class RequestTypes
{
    public static class Activities
    {
        public static readonly RequestType Archive = RequestType.Parse("api.activities.archive", null);
        public static readonly RequestType Create = RequestType.Parse("api.activities.create", null);
        public static readonly RequestType Get = RequestType.Parse("api.activities.get", null);
        public static readonly RequestType List = RequestType.Parse("api.activities.list", null);
        public static readonly RequestType Remove = RequestType.Parse("api.activities.remove", null);
        public static readonly RequestType SearchByName = RequestType.Parse("api.activities.search-by-name", null);
        public static readonly RequestType Unarchive = RequestType.Parse("api.activities.unarchive", null);
        public static readonly RequestType Update = RequestType.Parse("api.activities.update", null);
    }

    public static class Auth
    {
        public static readonly RequestType Login = RequestType.Parse("api.auth.login", null);
        public static readonly RequestType Logout = RequestType.Parse("api.auth.logout", null);
        public static readonly RequestType Refresh = RequestType.Parse("api.auth.refresh", null);
        public static readonly RequestType Register = RequestType.Parse("api.auth.register", null);
    }

    public static class DataTransfer
    {
        public static class Export
        {
            public static readonly RequestType Data = RequestType.Parse("api.data-transfer.exports.data", null);
        }

        public static class Formats
        {
            public static readonly RequestType List = RequestType.Parse("api.data-transfer.formats.list", null);
        }

        public static class Import
        {
            public static readonly RequestType Apply = RequestType.Parse("api.data-transfer.imports.apply", null);
            public static readonly RequestType Get = RequestType.Parse("api.data-transfer.imports.get", null);
            public static readonly RequestType List = RequestType.Parse("api.data-transfer.imports.list", null);
            public static readonly RequestType ListActivities = RequestType.Parse("api.data-transfer.imports.list-activities", null);
            public static readonly RequestType ListRows = RequestType.Parse("api.data-transfer.imports.list-rows", null);
            public static readonly RequestType ListTags = RequestType.Parse("api.data-transfer.imports.list-tags", null);
            public static readonly RequestType Remove = RequestType.Parse("api.data-transfer.imports.remove", null);
            public static readonly RequestType UpdateActivityMapping = RequestType.Parse("api.data-transfer.imports.update-activity-mapping", null);
            public static readonly RequestType UpdateTagMapping = RequestType.Parse("api.data-transfer.imports.update-tag-mapping", null);
            public static readonly RequestType Upload = RequestType.Parse("api.data-transfer.imports.upload", null);
        }
    }

    public static class Events
    {
        public static readonly RequestType Get = RequestType.Parse("api.events.get", null);
        public static readonly RequestType List = RequestType.Parse("api.events.list", null);
    }

    public static class Records
    {
        public static readonly RequestType AddPeriod = RequestType.Parse("api.records.add-period", null);
        public static readonly RequestType Create = RequestType.Parse("api.records.create", null);
        public static readonly RequestType Get = RequestType.Parse("api.records.get", null);
        public static readonly RequestType List = RequestType.Parse("api.records.list", null);
        public static readonly RequestType ListByPeriod = RequestType.Parse("api.records.list-by-period", null);
        public static readonly RequestType Remove = RequestType.Parse("api.records.remove", null);
        public static readonly RequestType RemovePeriod = RequestType.Parse("api.records.remove-period", null);
        public static readonly RequestType Update = RequestType.Parse("api.records.update", null);
        public static readonly RequestType UpdatePeriod = RequestType.Parse("api.records.update-period", null);
    }

    public static class RunningRecords
    {
        public static readonly RequestType Get = RequestType.Parse("api.running-records.get", null);
        public static readonly RequestType Remove = RequestType.Parse("api.running-records.remove", null);
        public static readonly RequestType Start = RequestType.Parse("api.running-records.start", null);
        public static readonly RequestType Stop = RequestType.Parse("api.running-records.stop", null);
        public static readonly RequestType Update = RequestType.Parse("api.running-records.update", null);
    }

    public static class Statistics
    {
        public static readonly RequestType PerDays = RequestType.Parse("api.statistics.per-days", null);
        public static readonly RequestType PerPeriod = RequestType.Parse("api.statistics.per-period", null);
    }

    public static class Tags
    {
        public static readonly RequestType Create = RequestType.Parse("api.tags.create", null);
        public static readonly RequestType Get = RequestType.Parse("api.tags.get", null);
        public static readonly RequestType List = RequestType.Parse("api.tags.list", null);
        public static readonly RequestType Remove = RequestType.Parse("api.tags.remove", null);
        public static readonly RequestType SearchByName = RequestType.Parse("api.tags.search-by-name", null);
        public static readonly RequestType Update = RequestType.Parse("api.tags.update", null);
    }

    public static class Users
    {
        public static readonly RequestType Get = RequestType.Parse("api.users.get", null);
        public static readonly RequestType Register = RequestType.Parse("api.users.register", null);

        public static class Settings
        {
            public static readonly RequestType Get = RequestType.Parse("api.users.settings.get", null);
            public static readonly RequestType Update = RequestType.Parse("api.users.settings.update", null);
        }
    }
}
