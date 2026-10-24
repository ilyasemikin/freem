using Freem.Domain.Import;
using Freem.Domain.Activities.Models;

namespace Freem.Domain.Entities.Import;

public sealed class ImportActivity
{
    public ImportActivityId Id { get; }
    public ActivityName Name { get; }
    public ActivityColor? Color { get; }
    public ActivityStatus Status { get; }
    public IReadOnlyList<ImportTagId> TagIds { get; }

    public ImportActivity(
        ImportActivityId id,
        ActivityName name,
        ActivityColor? color,
        ActivityStatus status,
        IReadOnlyList<ImportTagId> tagIds)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(status);
        ArgumentNullException.ThrowIfNull(tagIds);

        Id = id;
        Name = name;
        Color = color;
        Status = status;
        TagIds = tagIds;
    }
}
