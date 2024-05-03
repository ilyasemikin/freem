namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal class RecordEntity
{
    public required string Id { get; init; }

    public required DateTimeOffset StartAt { get; set; }
    public required DateTimeOffset EndAt { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    public IList<CategoryEntity>? Categories { get; set; }
    public IList<TagEntity>? Tags { get; set; }
}
