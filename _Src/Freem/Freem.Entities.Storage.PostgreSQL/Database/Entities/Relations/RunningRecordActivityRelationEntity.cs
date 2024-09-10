namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

internal sealed class RunningRecordActivityRelationEntity
{
    public required string RunningRecordUserId { get; init; }
    public required string ActivityId { get; init; }

    public RunningRecordEntity? RunningRecord { get; set; }
    public ActivityEntity? Activity { get; set; }
}
