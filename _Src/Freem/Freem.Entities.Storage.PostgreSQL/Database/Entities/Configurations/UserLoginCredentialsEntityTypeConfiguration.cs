using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class UserLoginCredentialsEntityTypeConfiguration : IEntityTypeConfiguration<UserPasswordCredentialsEntity>
{
    public void Configure(EntityTypeBuilder<UserPasswordCredentialsEntity> builder)
    {
        builder.ToTable(EntitiesNames.UsersLoginCredentials.Table);

        builder
            .Property(e => e.UserId)
            .HasColumnName(EntitiesNames.UsersLoginCredentials.Properties.UserId)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.Login)
            .HasColumnName(EntitiesNames.UsersLoginCredentials.Properties.Login)
            .IsRequired();
        
        builder
            .Property(e => e.HashAlgorithm)
            .HasColumnName(EntitiesNames.UsersLoginCredentials.Properties.HashAlgorithm)
            .IsRequired();

        builder
            .Property(e => e.PasswordHash)
            .HasColumnName(EntitiesNames.UsersLoginCredentials.Properties.PasswordHash)
            .IsRequired();

        builder
            .Property(e => e.PasswordSalt)
            .HasColumnName(EntitiesNames.UsersLoginCredentials.Properties.PasswordSalt)
            .IsRequired();

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(EntitiesNames.UsersLoginCredentials.Properties.CreatedAt)
            .IsRequired();
        
        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName(EntitiesNames.UsersLoginCredentials.Properties.UpdatedAt);
        
        builder
            .HasKey(e => e.UserId)
            .HasName(EntitiesNames.UsersLoginCredentials.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.Login)
            .HasDatabaseName(EntitiesNames.UsersLoginCredentials.Constraints.LoginIndex)
            .IsUnique()
            .HasDatabaseName(EntitiesNames.UsersLoginCredentials.Constraints.LoginUnique);
        
        builder
            .HasOne(e => e.User)
            .WithOne(e => e.PasswordCredentials)
            .HasForeignKey<UserPasswordCredentialsEntity>(e => e.UserId)
            .HasConstraintName(EntitiesNames.UsersLoginCredentials.Constraints.UsersForeignKey)
            .IsRequired();
    }
}