using Freem.Domain.DataTransfer.Formats;

namespace Freem.Domain.UseCases.Contracts.DataTransfer.Formats.List;

public sealed class ListFormatsResponse
{
    public IReadOnlyList<FormatDescription> ExportFormats { get; }
    public IReadOnlyList<FormatDescription> ImportFormats { get; }

    public ListFormatsResponse(
        IReadOnlyList<FormatDescription> exportFormats,
        IReadOnlyList<FormatDescription> importFormats)
    {
        ArgumentNullException.ThrowIfNull(exportFormats);
        ArgumentNullException.ThrowIfNull(importFormats);

        ExportFormats = exportFormats;
        ImportFormats = importFormats;
    }
}
