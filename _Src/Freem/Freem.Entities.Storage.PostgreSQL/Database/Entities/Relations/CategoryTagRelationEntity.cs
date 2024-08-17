namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

internal sealed class CategoryTagRelationEntity
{
    public required string CategoryId { get; init; }
    public required string TagId { get; init; }

    public CategoryEntity? Category { get; set; }
    public TagEntity? Tag { get; set; }
}
