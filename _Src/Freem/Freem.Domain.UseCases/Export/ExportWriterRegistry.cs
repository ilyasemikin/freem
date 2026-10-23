using Freem.Domain.Formats;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.Export;

public sealed class ExportWriterRegistry
{
    private readonly IReadOnlyDictionary<FormatKey, IExportWriter> _writers;

    public IReadOnlyList<FormatDescription> Formats { get; }

    public ExportWriterRegistry(IEnumerable<IExportWriter> writers)
    {
        ArgumentNullException.ThrowIfNull(writers);

        var items = writers.ToArray();
        var duplicate = items
            .GroupBy(writer => writer.Format.Key)
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicate is not null)
            throw new ArgumentException($"Export format key \"{duplicate.Key}\" is registered more than once", nameof(writers));

        _writers = items.ToDictionary(writer => writer.Format.Key);
        Formats = items
            .Select(writer => writer.Format)
            .OrderBy(format => format.Key.ToString(), StringComparer.Ordinal)
            .ToArray();
    }

    public Result<IExportWriter, Error> Find(FormatKey key)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (!_writers.TryGetValue(key, out var writer))
            return DomainErrorCatalog.Factories.CreateFormatNotFound(key);

        return Result<IExportWriter, Error>.CreateSuccess(writer);
    }
}
