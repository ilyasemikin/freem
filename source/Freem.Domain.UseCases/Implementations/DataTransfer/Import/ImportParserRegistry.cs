using Freem.Domain.DataTransfer.Formats;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Import;

public sealed class ImportParserRegistry
{
    private readonly IReadOnlyDictionary<FormatKey, IImportParser> _parsers;

    public IReadOnlyList<FormatDescription> Formats { get; }

    public ImportParserRegistry(IEnumerable<IImportParser> parsers)
    {
        ArgumentNullException.ThrowIfNull(parsers);

        var items = parsers.ToArray();
        var duplicate = items
            .GroupBy(parser => parser.Format.Key)
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicate is not null)
            throw new ArgumentException($"Import format key \"{duplicate.Key}\" is registered more than once", nameof(parsers));

        _parsers = items.ToDictionary(parser => parser.Format.Key);
        Formats = items
            .Select(parser => parser.Format)
            .OrderBy(format => format.Key.ToString(), StringComparer.Ordinal)
            .ToArray();
    }

    public Result<IImportParser, Error> Find(FormatKey key)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (!_parsers.TryGetValue(key, out var parser))
            return DomainErrorCatalog.Factories.CreateFormatNotFound(key);

        return Result<IImportParser, Error>.CreateSuccess(parser);
    }
}
