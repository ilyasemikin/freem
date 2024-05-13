using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Extensions;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.Database;

internal sealed class DatabaseContext : DbContext
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
        builder.HasDefaultSchema(EnvironmentNames.Schema);

        builder.ApplyEntitiesConfigurations();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            IAuditableEntity entity = entry.Entity;
            switch (entry.State)
            {
                case EntityState.Added:
                    entity.CreatedAt = entity.CreatedAt == DateTimeOffset.MinValue
                        ? now
                        : entity.CreatedAt;
                    break;
                case EntityState.Modified:
                    entity.UpdatedAt ??= now;
                    break;
            }
        }

        foreach (var entry in ChangeTracker.Entries<ISoftDeletedEntity>())
        {
            ISoftDeletedEntity entity = entry.Entity;
            switch (entry.State)
            {
                case EntityState.Deleted:
                    entity.DeletedAt = now;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
