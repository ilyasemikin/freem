namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

internal sealed class RecordActivityRelationEntity
{
    public required string RecordId { get; init; }
    public required string ActivityId { get; init; }

    public RecordEntity? Record { get; set; }
    public ActivityEntity? Activity { get; set; }
}
