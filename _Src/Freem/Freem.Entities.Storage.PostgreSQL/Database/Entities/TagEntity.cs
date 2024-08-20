using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal sealed class TagEntity : IRowVersionableEntity
{
    public required string Id { get; init; }
    public required string UserId { get; init; }

    public required string Name { get; set; }

    public UserEntity? User { get; set; }
    public ICollection<CategoryEntity>? Categories { get; set; }
    public ICollection<RecordEntity>? Records { get; set; }
    public ICollection<RunningRecordEntity>? RunningRecords { get; set; }

    [Timestamp]
    public uint RowVersion { get; private set; }
}
