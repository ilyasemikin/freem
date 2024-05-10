using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.Database.Relations;

internal class RunningRecordCategoryRelation
{
    public required string RunningRecordUserId { get; init; }
    public required string CategoryId { get; init; }

    public RunningRecordEntity? RunningRecord { get; set; }
    public CategoryEntity? Category { get; set; }
}