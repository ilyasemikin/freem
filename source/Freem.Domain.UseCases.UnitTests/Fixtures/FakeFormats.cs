using Freem.Domain.Entities.Export;
using Freem.Domain.Formats;
using Freem.Domain.Entities.Import;
using Freem.Domain.UseCases.Implementations.Export;
using Freem.Domain.UseCases.Implementations.Import;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.UnitTests.Fixtures;

internal sealed class FakeExportWriter : IExportWriter
{
    public FormatDescription Format { get; }

    public FakeExportWriter(string key)
    {
        var formatKey = FormatKey.Parse(key, null);
        var name = FormatName.Parse(key, null);
        var fileExtension = FormatFileExtension.Parse("txt", null);
        var mediaType = FormatMediaType.Parse("text/plain", null);
        Format = new FormatDescription(formatKey, name, fileExtension, mediaType);
    }

    public Task WriteAsync(
        IAsyncEnumerable<ExportItem> items,
        Stream output,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}

internal sealed class FakeImportParser : IImportParser
{
    public FormatDescription Format { get; }

    public FakeImportParser(string key)
    {
        var formatKey = FormatKey.Parse(key, null);
        var name = FormatName.Parse(key, null);
        var fileExtension = FormatFileExtension.Parse("txt", null);
        var mediaType = FormatMediaType.Parse("text/plain", null);
        Format = new FormatDescription(formatKey, name, fileExtension, mediaType);
    }

    public Task<Result<ImportParseResult, Error>> ParseAsync(
        Stream input,
        ImportParseOptions options,
        CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}
