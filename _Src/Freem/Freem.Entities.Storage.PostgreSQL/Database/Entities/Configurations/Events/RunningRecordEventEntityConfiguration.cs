using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations.Events;

internal class RunningRecordEventEntityConfiguration : IEntityTypeConfiguration<RunningRecordEventEntity>
{
    public void Configure(EntityTypeBuilder<RunningRecordEventEntity> builder)
    {
    }
}