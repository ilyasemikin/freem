using Freem.Domain.Formats;
using Freem.Domain.Entities.Import;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.Import;

public interface IImportParser
{
    FormatDescription Format { get; }

    Task<Result<ImportDocument, Error>> ParseAsync(
        Stream input,
        CancellationToken cancellationToken = default);
}
