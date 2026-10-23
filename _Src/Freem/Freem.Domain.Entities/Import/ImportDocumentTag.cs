using Freem.Domain.Import;

namespace Freem.Domain.Entities.Import;

public sealed class ImportDocumentTag
{
    public ImportTag Source { get; }
    public ImportTagResolution? Resolution { get; }

    public ImportDocumentTag(ImportTag source, ImportTagResolution? resolution = null)
    {
        ArgumentNullException.ThrowIfNull(source);

        Source = source;
        Resolution = resolution;
    }
}
