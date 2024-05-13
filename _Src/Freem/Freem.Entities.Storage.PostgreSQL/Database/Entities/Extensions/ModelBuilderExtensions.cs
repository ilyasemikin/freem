using Freem.EFCore.Extensions;
using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Extensions;

internal static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyEntitiesConfigurations(this ModelBuilder builder)
    {
        builder.ApplyRelationEnttitiesConfiguration();

        builder.HasPostgresEnum<CategoryStatus>(EnvironmentNames.Schema, EntitiesNames.Categories.Models.CategoryStatus);

        builder.ApplyConfiguration<CategoryEntity, CategoryEntityTypeConfiguration>();

        builder.ApplyConfiguration<RecordEntity, RecordEntityTypeConfiguration>();
        builder.ApplyConfiguration<RunningRecordEntity, RunningRecordEntityTypeConfiguration>();

        builder.ApplyConfiguration<TagEntity, TagEntityTypeConfiguration>();

        builder.ApplyConfiguration<UserEntity, UserEntityTypeConfiguration>();

        return builder;
    }
}
