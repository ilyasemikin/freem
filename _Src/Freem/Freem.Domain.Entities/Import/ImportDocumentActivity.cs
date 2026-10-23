using Freem.Domain.Import;

namespace Freem.Domain.Entities.Import;

public sealed class ImportDocumentActivity
{
    public ImportActivity Source { get; }
    public ImportActivityResolution? Resolution { get; }

    public ImportDocumentActivity(ImportActivity source, ImportActivityResolution? resolution = null)
    {
        ArgumentNullException.ThrowIfNull(source);

        Source = source;
        Resolution = resolution;
    }
}
