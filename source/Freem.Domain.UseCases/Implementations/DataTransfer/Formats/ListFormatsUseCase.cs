using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.DataTransfer.Formats.List;
using Freem.Domain.UseCases.Implementations.DataTransfer.Export;
using Freem.Domain.UseCases.Implementations.DataTransfer.Import;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Formats;

internal sealed class ListFormatsUseCase
    : IRequestHandler<HandlerContext, ListFormatsRequest, Result<ListFormatsResponse, Error>>
{
    private readonly ExportWriterRegistry _writers;
    private readonly ImportParserRegistry _parsers;

    public ListFormatsUseCase(
        ExportWriterRegistry writers,
        ImportParserRegistry parsers)
    {
        ArgumentNullException.ThrowIfNull(writers);
        ArgumentNullException.ThrowIfNull(parsers);

        _writers = writers;
        _parsers = parsers;
    }

    public Task<Result<ListFormatsResponse, Error>> HandleAsync(
        HandlerContext context, ListFormatsRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = new ListFormatsResponse(_writers.Formats, _parsers.Formats);
        Result<ListFormatsResponse, Error> result = response;
        return Task.FromResult(result);
    }
}
