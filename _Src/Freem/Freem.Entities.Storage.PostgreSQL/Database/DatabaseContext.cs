using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations.Extensions;
using Freem.Entities.Storage.PostgreSQL.Database.Relations.Configurations.Extensions;

namespace Freem.Entities.Storage.PostgreSQL.Database;

internal class DatabaseContext : DbContext
{
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    public DbSet<RecordEntity> Records => Set<RecordEntity>();
    public DbSet<RunningRecordEntity> RunningRecords => Set<RunningRecordEntity>();

    public DbSet<TagEntity> Tags => Set<TagEntity>();

    public DbSet<UserEntity> Users => Set<UserEntity>();

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyRelationsConfigurations();
        builder.ApplyEntitiesConfigurations();

        base.OnModelCreating(builder);
    }
}
