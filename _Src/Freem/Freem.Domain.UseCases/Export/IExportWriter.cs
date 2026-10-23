using Freem.Domain.Entities.Export;
using Freem.Domain.Formats;

namespace Freem.Domain.UseCases.Export;

public interface IExportWriter
{
    FormatDescription Format { get; }

    Task WriteAsync(
        IAsyncEnumerable<ExportItem> items,
        Stream output,
        CancellationToken cancellationToken = default);
}
