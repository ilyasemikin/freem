namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal class RunningRecordEntity
{
    public required string UserId { get; set; }
    
    public string? Name { get; set; }
    public string? Description { get; set; }

    public DateTimeOffset StartAt { get; set; }

    public UserEntity? User { get; set; }
    public IList<CategoryEntity>? Categories { get; set; }
    public IList<TagEntity>? TagIds { get; set; }
}
