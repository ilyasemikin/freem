using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable(EntitiesNames.Users.Table);

        builder
            .Property(e => e.Id)
            .HasColumnName(EntitiesNames.Users.Properties.Id)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.Nickname)
            .HasColumnName(EntitiesNames.Users.Properties.Nickname)
            .IsRequired();

        builder
            .HasKey(e => e.Id)
            .HasName(EntitiesNames.Users.Constraints.PrimaryKey);

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(EntitiesNames.Users.Properties.CreatedAt)
            .HasColumnOrder(1)
            .IsRequired();

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName(EntitiesNames.Users.Properties.UpdatedAt);

        builder
            .Property(e => e.DeletedAt)
            .HasColumnName(EntitiesNames.Users.Properties.DeletedAt);
    }
}
