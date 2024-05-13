using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Constants;

internal class RelationsNames
{
    public static class CategoriesTags
    {
        public const string Table = $"{EntitiesNames.Categories.Table}_{EntitiesNames.Tags.Table}";

        public static class Properties
        {
            public const string CategoryId = "category_id";
            public const string TagId = "tag_id";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string CategoriesForeignKey = $"{Table}_{EntitiesNames.Categories.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TagsForeignKey = $"{Table}_{EntitiesNames.Tags.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TagIdIndex = $"{Table}_{Properties.TagId}_{NamingConvention.IndexSuffix}";
        }
    }

    public static class RecordsTags
    {
        public const string Table = $"{EntitiesNames.Records.Table}_{EntitiesNames.Tags.Table}";

        public static class Properties
        {
            public const string RecordId = "record_id";
            public const string TagId = "tag_id";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string RecordsForeignKey = $"{Table}_{EntitiesNames.Records.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TagsForeignKey = $"{Table}_{EntitiesNames.Tags.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TagIdIndex = $"{Table}_{Properties.TagId}_{NamingConvention.IndexSuffix}";
        }
    }

    public static class RunningRecordsTags
    {
        public const string Table = $"{EntitiesNames.RunningRecords.Table}_{EntitiesNames.Tags.Table}";

        public static class Properties
        {
            public const string RunningRecordId = $"user_id";
            public const string TagId = "tag_id";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string RunningRecordsForeignKey = $"{Table}_{EntitiesNames.RunningRecords.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TagsForeignKey = $"{Table}_{EntitiesNames.Tags.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TagIdIndex = $"{Table}_{Properties.TagId}_{NamingConvention.IndexSuffix}";
        }
    }

    public static class RecordsCategories
    {
        public const string Table = $"{EntitiesNames.Records.Table}_{EntitiesNames.Categories.Table}";

        public static class Properties
        {
            public const string RecordId = "record_id";
            public const string CategoryId = "category_id";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string RecordsForeignKey = $"{Table}_{EntitiesNames.Records.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string CategoriesForeignKey = $"{Table}_{EntitiesNames.Categories.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string CategoryIdIndex = $"{Table}_{Properties.CategoryId}_{NamingConvention.IndexSuffix}";
        }
    }

    public static class RunningRecordsCategories
    {
        public const string Table = $"{EntitiesNames.RunningRecords.Table}_{EntitiesNames.Categories.Table}";

        public static class Properties
        {
            public const string RunningRecordId = "user_id";
            public const string CategoryId = "category_id";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string RunningRecordsForeignKey = $"{Table}_{EntitiesNames.RunningRecords.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string CategoriesForeignKey = $"{Table}_{EntitiesNames.Categories.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string CategoryIdIndex = $"{Table}_{Properties.CategoryId}_{NamingConvention.IndexSuffix}";
        }
    }
}
