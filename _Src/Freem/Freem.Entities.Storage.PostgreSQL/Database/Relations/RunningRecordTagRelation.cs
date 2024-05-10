using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.Database.Relations;

internal class RunningRecordTagRelation
{
    public required string RunningRecordUserId { get; init; }
    public required string TagId { get; init; }

    public RunningRecordEntity? RunningRecord { get; set; }
    public TagEntity? Tag { get; set; }
}