using Freem.Domain.Storage.PostgreSQL.Database.Entities;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.DataFactories;

internal static class DatabaseEntityCopyExtensions
{
    public static ActivityEntity Copy(this ActivityEntity entity)
    {
        return new ActivityEntity
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Name = entity.Name,
            Status = entity.Status,
            User = entity.User,
            Tags = entity.Tags?.ToList(),
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public static RecordEntity Copy(this RecordEntity entity)
    {
        return new RecordEntity
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Name = entity.Name,
            Description = entity.Description,
            Periods = entity.Periods?.ToList(),
            User = entity.User,
            Activities = entity.Activities?.ToList(),
            Tags = entity.Tags?.ToList(),
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public static RunningRecordEntity Copy(this RunningRecordEntity entity)
    {
        return new RunningRecordEntity
        {
            UserId = entity.UserId,
            Name = entity.Name,
            Description = entity.Description,
            StartAt = entity.StartAt,
            User = entity.User,
            Activities = entity.Activities?.ToList(),
            Tags = entity.Tags?.ToList(),
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }

    public static TagEntity Copy(this TagEntity entity)
    {
        return new TagEntity
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Name = entity.Name,
            User = entity.User,
            Activities = entity.Activities?.ToList(),
            Records = entity.Records?.ToList(),
            RunningRecords = entity.RunningRecords?.ToList(),
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}