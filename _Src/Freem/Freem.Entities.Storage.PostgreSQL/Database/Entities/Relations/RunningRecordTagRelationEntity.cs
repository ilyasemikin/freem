namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

internal sealed class RunningRecordTagRelationEntity
{
    public required string RunningRecordUserId { get; init; }
    public required string TagId { get; init; }

    public RunningRecordEntity? RunningRecord { get; set; }
    public TagEntity? Tag { get; set; }
}
