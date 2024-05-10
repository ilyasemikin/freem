using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.Database.Relations;

internal class RecordTagRelation
{
    public required string RecordId { get; init; }
    public required string TagId { get; init; }

    public RecordEntity? Record { get; set; }
    public TagEntity? Tag { get; set; }
}