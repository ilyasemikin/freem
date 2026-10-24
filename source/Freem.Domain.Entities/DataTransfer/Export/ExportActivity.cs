using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Tags;

namespace Freem.Domain.Entities.DataTransfer.Export;

public sealed class ExportActivity : ExportItem
{
    public ActivityId Id { get; }
    public ActivityName Name { get; }
    public ActivityColor Color { get; }
    public ActivityStatus Status { get; }
    public IReadOnlyList<TagId> TagIds { get; }

    public ExportActivity(
        ActivityId id,
        ActivityName name,
        ActivityColor color,
        ActivityStatus status,
        IReadOnlyList<TagId> tagIds)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(color);
        ArgumentNullException.ThrowIfNull(status);
        ArgumentNullException.ThrowIfNull(tagIds);

        Id = id;
        Name = name;
        Color = color;
        Status = status;
        TagIds = tagIds;
    }
}
