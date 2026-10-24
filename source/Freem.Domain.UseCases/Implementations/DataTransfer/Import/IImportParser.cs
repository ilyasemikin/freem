using Freem.Domain.Entities.DataTransfer.Import;
using Freem.Domain.DataTransfer.Formats;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Import;

public interface IImportParser
{
    FormatDescription Format { get; }

    Task<Result<ImportParseResult, Error>> ParseAsync(
        Stream input,
        ImportParseOptions options,
        CancellationToken cancellationToken = default);
}
