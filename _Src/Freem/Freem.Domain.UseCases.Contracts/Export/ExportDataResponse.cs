using Freem.Domain.Formats;

namespace Freem.Domain.UseCases.Contracts.Export;

public sealed class ExportDataResponse
{
    public FormatDescription Format { get; }

    public ExportDataResponse(FormatDescription format)
    {
        ArgumentNullException.ThrowIfNull(format);

        Format = format;
    }
}
