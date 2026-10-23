using Freem.Domain.Formats;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Export;

public sealed class ExportDataRequest : IRequest
{
    public RequestType Type => RequestTypes.Export.Data;

    public FormatKey Format { get; }
    public Func<FormatDescription, Stream> OpenOutput { get; }

    public ExportDataRequest(FormatKey format, Func<FormatDescription, Stream> openOutput)
    {
        ArgumentNullException.ThrowIfNull(format);
        ArgumentNullException.ThrowIfNull(openOutput);

        Format = format;
        OpenOutput = openOutput;
    }
}
