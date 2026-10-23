using System.Net.Mime;
using System.Text;
using Utilities.Http.Content.Abstractions;

namespace Freem.Api.Public.Client.Implementations.Base;

/// <summary>A <c>multipart/form-data</c> body with text fields and one file.</summary>
internal sealed class ImportFormContent : IHttpRequestContent
{
    private static readonly Encoding TextEncoding = new UTF8Encoding(false);

    private readonly string _boundary;
    private readonly IReadOnlyList<KeyValuePair<string, string>> _fields;
    private readonly string _fileField;
    private readonly string _fileName;
    private readonly Stream _file;

    public ContentType ContentType { get; }

    public ImportFormContent(
        IEnumerable<KeyValuePair<string, string>> fields,
        string fileField,
        string fileName,
        Stream file)
    {
        ArgumentNullException.ThrowIfNull(fields);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileField);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentNullException.ThrowIfNull(file);

        _boundary = $"----------------{Guid.NewGuid():N}";
        _fields = fields.ToArray();
        _fileField = fileField;
        _fileName = fileName;
        _file = file;

        ContentType = new ContentType($"multipart/form-data; boundary={_boundary}");
    }

    public async Task SerializeToStreamAsync(Stream stream, CancellationToken ct = default)
    {
        foreach (var field in _fields)
        {
            await WriteAsync(stream, $"--{_boundary}\r\n", ct);
            await WriteAsync(stream, $"Content-Disposition: form-data; name=\"{Escape(field.Key)}\"\r\n\r\n", ct);
            await WriteAsync(stream, $"{field.Value}\r\n", ct);
        }

        await WriteAsync(stream, $"--{_boundary}\r\n", ct);
        await WriteAsync(
            stream,
            $"Content-Disposition: form-data; name=\"{Escape(_fileField)}\"; filename=\"{Escape(_fileName)}\"\r\n",
            ct);
        await WriteAsync(stream, "Content-Type: application/octet-stream\r\n\r\n", ct);
        await _file.CopyToAsync(stream, ct);
        await WriteAsync(stream, "\r\n", ct);

        await WriteAsync(stream, $"--{_boundary}--\r\n", ct);
    }

    private static Task WriteAsync(Stream stream, string value, CancellationToken ct)
    {
        var bytes = TextEncoding.GetBytes(value);
        return stream.WriteAsync(bytes, 0, bytes.Length, ct);
    }

    private static string Escape(string value)
    {
        return value
            .Replace("\"", "%22")
            .Replace("\r", "%0D")
            .Replace("\n", "%0A");
    }
}
