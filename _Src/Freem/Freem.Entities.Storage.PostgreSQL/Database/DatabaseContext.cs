using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Microsoft.EntityFrameworkCore;

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
}
