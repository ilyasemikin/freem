using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations.Events;

internal class CategoryEventEntityConfiguration : IEntityTypeConfiguration<CategoryEventEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEventEntity> builder)
    {
        builder
            .Property(e => e.CategoryId)
            .HasColumnName(EntitiesNames.Events.Categories.Properties.CategoryId);
    }
}