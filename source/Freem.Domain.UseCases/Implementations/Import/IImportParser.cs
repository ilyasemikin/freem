using Freem.Domain.Entities.Import;
using Freem.Domain.Formats;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.Implementations.Import;

public interface IImportParser
{
    FormatDescription Format { get; }

    Task<Result<ImportParseResult, Error>> ParseAsync(
        Stream input,
        ImportParseOptions options,
        CancellationToken cancellationToken = default);
}
