using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations.Events;

internal class RecordEventEntityConfiguration : IEntityTypeConfiguration<RecordEventEntity>
{
    public void Configure(EntityTypeBuilder<RecordEventEntity> builder)
    {
        builder
            .Property(e => e.RecordId)
            .HasColumnName(EntitiesNames.Events.Records.Properties.RecordId);
    }
}