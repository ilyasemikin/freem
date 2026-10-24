using Freem.Domain.DataTransfer.Formats;

namespace Freem.Domain.UseCases.Contracts.DataTransfer.Export;

public sealed class ExportDataResponse
{
    public FormatDescription Format { get; }

    public ExportDataResponse(FormatDescription format)
    {
        ArgumentNullException.ThrowIfNull(format);

        Format = format;
    }
}
