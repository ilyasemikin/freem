using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;

namespace Freem.Domain.Entities.DataTransfer.Export;

public sealed class ExportTag : ExportItem
{
    public TagId Id { get; }
    public TagName Name { get; }

    public ExportTag(TagId id, TagName name)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(name);

        Id = id;
        Name = name;
    }
}
