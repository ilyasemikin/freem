using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Imports;

public sealed class ApiUploadImportRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.Upload;

    public string Format { get; }
    public string FileName { get; }
    public string? TimeZoneOffset { get; }
    public Stream Content { get; }

    public ApiUploadImportRequest(string format, string fileName, string? timeZoneOffset, Stream content)
    {
        ArgumentNullException.ThrowIfNull(format);
        ArgumentNullException.ThrowIfNull(fileName);
        ArgumentNullException.ThrowIfNull(content);

        Format = format;
        FileName = fileName;
        TimeZoneOffset = timeZoneOffset;
        Content = content;
    }
}
