namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

internal sealed class RunningRecordCategoryRelationEntity
{
    public required string RunningRecordUserId { get; init; }
    public required string CategoryId { get; init; }

    public RunningRecordEntity? RunningRecord { get; set; }
    public CategoryEntity? Category { get; set; }
}
