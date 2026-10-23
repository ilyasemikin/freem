using Freem.Domain.Formats;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.DataTransfer;

public sealed class ApiExportDataRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Export;

    public FormatKey Format { get; }
    public Func<FormatDescription, Stream> OpenOutput { get; }

    public ApiExportDataRequest(FormatKey format, Func<FormatDescription, Stream> openOutput)
    {
        ArgumentNullException.ThrowIfNull(format);
        ArgumentNullException.ThrowIfNull(openOutput);

        Format = format;
        OpenOutput = openOutput;
    }
}
