using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Export;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Export;

internal sealed class ExportDataUseCase
    : IRequestHandler<HandlerContext, ExportDataRequest, Result<ExportDataResponse, Error>>
{
    private readonly ExportWriterRegistry _writers;
    private readonly ExportItemReader _reader;

    public ExportDataUseCase(
        ExportWriterRegistry writers,
        ExportItemReader reader)
    {
        ArgumentNullException.ThrowIfNull(writers);
        ArgumentNullException.ThrowIfNull(reader);

        _writers = writers;
        _reader = reader;
    }

    public async Task<Result<ExportDataResponse, Error>> HandleAsync(
        HandlerContext context, ExportDataRequest request,
        CancellationToken cancellationToken = default)
    {
        var writerResult = _writers.Find(request.Format);
        if (!writerResult.Success)
            return writerResult.Error;

        var writer = writerResult.Value;
        var output = request.OpenOutput(writer.Format);
        var items = _reader.ReadAsync(context.UserId, cancellationToken);

        try
        {
            await writer.WriteAsync(items, output, cancellationToken);
        }
        catch (ExportReadException exception)
        {
            return exception.Error;
        }

        return new ExportDataResponse(writer.Format);
    }
}
