using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class UserTelegramIntegrationEntityTypeConfiguration : IEntityTypeConfiguration<UserTelegramIntegrationEntity>
{
    public void Configure(EntityTypeBuilder<UserTelegramIntegrationEntity> builder)
    {
        builder.ToTable(EntitiesNames.UserTelegramIntegration.Table);

        builder
            .Property(e => e.UserId)
            .HasColumnName(EntitiesNames.UserTelegramIntegration.Properties.UserId)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.TelegramUserId)
            .HasColumnName(EntitiesNames.UserTelegramIntegration.Properties.TelegramUserId)
            .IsRequired();

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(EntitiesNames.UserTelegramIntegration.Properties.CreatedAt)
            .IsRequired();

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName(EntitiesNames.UserTelegramIntegration.Properties.UpdatedAt)
            .IsRequired();
        
        builder
            .HasKey(e => e.UserId)
            .HasName(EntitiesNames.UserTelegramIntegration.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.TelegramUserId)
            .HasDatabaseName(EntitiesNames.UserTelegramIntegration.Constraints.TelegramUserIdIndex)
            .IsUnique()
            .HasDatabaseName(EntitiesNames.UserTelegramIntegration.Constraints.TelegramUserIdUnique);
        
        builder
            .HasOne(e => e.User)
            .WithOne(e => e.TelegramIntegration)
            .HasForeignKey<UserTelegramIntegrationEntity>(e => e.UserId)
            .HasConstraintName(EntitiesNames.UserTelegramIntegration.Constraints.UsersForeignKey)
            .IsRequired();
    }
}