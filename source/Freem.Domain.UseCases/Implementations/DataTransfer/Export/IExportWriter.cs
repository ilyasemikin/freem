using Freem.Domain.Entities.DataTransfer.Export;
using Freem.Domain.DataTransfer.Formats;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Export;

public interface IExportWriter
{
    FormatDescription Format { get; }

    Task WriteAsync(
        IAsyncEnumerable<ExportItem> items,
        Stream output,
        CancellationToken cancellationToken = default);
}
