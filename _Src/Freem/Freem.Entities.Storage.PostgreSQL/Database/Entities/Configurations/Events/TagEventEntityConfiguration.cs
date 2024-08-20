using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations.Events;

internal class TagEventEntityConfiguration : IEntityTypeConfiguration<TagEventEntity>
{
    public void Configure(EntityTypeBuilder<TagEventEntity> builder)
    {
        builder
            .Property(e => e.TagId)
            .HasColumnName(EntitiesNames.Events.Tags.Properties.TagId);
    }
}