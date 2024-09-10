using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations.Events;

internal class ActivityEventEntityConfiguration : IEntityTypeConfiguration<ActivityEventEntity>
{
    public void Configure(EntityTypeBuilder<ActivityEventEntity> builder)
    {
        builder
            .Property(e => e.ActivityId)
            .HasColumnName(EntitiesNames.Events.Activities.Properties.ActivityId);
    }
}