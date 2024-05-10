using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.Database.Relations;

internal class RecordCategoryRelation
{
    public required string RecordId { get; init; }
    public required string CategoryId { get; init; }

    public RecordEntity? Record { get; set; }
    public CategoryEntity? Category { get; set; }
}