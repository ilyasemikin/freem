using Freem.Domain.DataTransfer.Formats;
using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Users.Models;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.DataTransfer.Import.Upload;

public sealed class UploadImportRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Import.Upload;

    public FormatKey Format { get; }
    public ImportFileName FileName { get; }
    public Stream Content { get; }
    public TimeZoneOffset? TimeZoneOffset { get; }

    public UploadImportRequest(
        FormatKey format,
        ImportFileName fileName,
        Stream content,
        TimeZoneOffset? timeZoneOffset = null)
    {
        ArgumentNullException.ThrowIfNull(format);
        ArgumentNullException.ThrowIfNull(fileName);
        ArgumentNullException.ThrowIfNull(content);

        Format = format;
        FileName = fileName;
        Content = content;
        TimeZoneOffset = timeZoneOffset;
    }
}
