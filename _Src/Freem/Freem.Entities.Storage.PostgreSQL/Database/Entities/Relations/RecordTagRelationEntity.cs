namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

internal sealed class RecordTagRelationEntity
{
    public required string RecordId { get; init; }
    public required string TagId { get; init; }

    public RecordEntity? Record { get; set; }
    public TagEntity? Tag { get; set; }
}
