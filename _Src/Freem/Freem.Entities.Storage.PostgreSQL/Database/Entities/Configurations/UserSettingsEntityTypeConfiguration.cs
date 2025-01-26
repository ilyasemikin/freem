using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class UserSettingsEntityTypeConfiguration : IEntityTypeConfiguration<UserSettingsEntity>
{
    public void Configure(EntityTypeBuilder<UserSettingsEntity> builder)
    {
        builder.ToTable(EntitiesNames.UserSettings.Table);

        builder
            .Property(e => e.UserId)
            .HasColumnName(EntitiesNames.UserSettings.Properties.UserId)
            .IsRequired();

        builder
            .Property(e => e.UtcOffsetTicks)
            .HasColumnName(EntitiesNames.UserSettings.Properties.UtcOffsetTicks)
            .IsRequired();

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(EntitiesNames.UserSettings.Properties.CreatedAt)
            .IsRequired();

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName(EntitiesNames.UserSettings.Properties.UpdatedAt);
        
        builder
            .HasKey(e => e.UserId)
            .HasName(EntitiesNames.UserSettings.Constraints.PrimaryKey);

        builder
            .HasOne(e => e.User)
            .WithOne(e => e.Settings)
            .HasForeignKey<UserSettingsEntity>(e => e.UserId)
            .HasConstraintName(EntitiesNames.UserSettings.Constraints.UsersForeignKey)
            .IsRequired();
    }
}