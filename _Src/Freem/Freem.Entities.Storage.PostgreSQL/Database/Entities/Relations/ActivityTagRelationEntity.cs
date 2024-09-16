namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

internal sealed class ActivityTagRelationEntity
{
    public required string ActivityId { get; init; }
    public required string TagId { get; init; }

    public ActivityEntity? Activity { get; set; }
    public TagEntity? Tag { get; set; }
}
