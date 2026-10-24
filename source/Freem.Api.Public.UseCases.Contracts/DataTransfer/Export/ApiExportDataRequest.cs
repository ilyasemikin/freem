using Freem.Domain.DataTransfer.Formats;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.DataTransfer.Export;

public sealed class ApiExportDataRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Export.Data;

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
