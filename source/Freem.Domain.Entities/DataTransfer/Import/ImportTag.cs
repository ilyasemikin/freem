using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Tags.Models;

namespace Freem.Domain.Entities.DataTransfer.Import;

public sealed class ImportTag
{
    public ImportTagId Id { get; }
    public TagName Name { get; }

    public ImportTag(ImportTagId id, TagName name)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(name);

        Id = id;
        Name = name;
    }
}
