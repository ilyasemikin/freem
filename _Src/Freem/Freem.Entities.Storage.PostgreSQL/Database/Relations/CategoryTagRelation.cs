using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.Database.Relations;

internal class CategoryTagRelation
{
    public required string CategoryId { get; init; }
    public required string TagId { get; init; }

    public CategoryEntity? Category { get; set; }
    public TagEntity? Tag { get; set; }
}