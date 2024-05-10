using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable(Names.Tables.Users, Names.Schema);

        builder
            .Property(e => e.Id)
            .HasColumnName(Names.Properties.Users.Id)
            .IsRequired();

        builder
            .Property(e => e.Nickname)
            .HasColumnName(Names.Properties.Users.Nickname)
            .IsRequired();

        builder
            .HasKey(e => e.Id)
            .HasName(Names.Constrains.Users.PrimaryKey);

        builder
            .HasIndex(e => e.Nickname)
            .HasDatabaseName(Names.Constrains.Users.NicknameIndex)
            .IsUnique()
            .HasDatabaseName(Names.Constrains.Users.NicknameUnique);
    }
}
