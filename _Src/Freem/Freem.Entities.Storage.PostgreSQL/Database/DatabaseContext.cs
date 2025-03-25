using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Database;

internal sealed class DatabaseContext : DbContext
{
    public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();

    public DbSet<RecordEntity> Records => Set<RecordEntity>();
    public DbSet<RunningRecordEntity> RunningRecords => Set<RunningRecordEntity>();

    public DbSet<TagEntity> Tags => Set<TagEntity>();

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<UserSettingsEntity> UserSettings => Set<UserSettingsEntity>();
    public DbSet<UserPasswordCredentialsEntity> UserLoginCredentials => Set<UserPasswordCredentialsEntity>();
    public DbSet<UserTelegramIntegrationEntity> UserTelegramIntegrations => Set<UserTelegramIntegrationEntity>();
    
    public DbSet<EventEntity> Events => Set<EventEntity>();

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // TODO: Использовать время из сервиса по получению текущего времени
        var now = DateTimeOffset.UtcNow;

        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            var entity = entry.Entity;
            switch (entry.State)
            {
                case EntityState.Added:
                    entity.CreatedAt = now;
                    break;
                case EntityState.Modified:
                    entity.UpdatedAt = now;
                    break;
                default:
                    continue;
            }
        }
        
        foreach (var entry in ChangeTracker.Entries<ISoftDeletedEntity>())
        {
            var entity = entry.Entity;
            if (entry.State is EntityState.Deleted)
                entity.DeletedAt = now;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(EnvironmentNames.Schema);

        builder.ApplyEntitiesConfigurations();
    }
}
