using Freem.Domain.Storage.Repositories.DataTransfer.Import;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.ListRows;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Import;

internal sealed class ListImportRowUseCase
    : IRequestHandler<HandlerContext, ListImportRowRequest, Result<ListImportRowResponse, Error>>
{
    private readonly IImportDocumentRepository _repository;

    public ListImportRowUseCase(IImportDocumentRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task<Result<ListImportRowResponse, Error>> HandleAsync(
        HandlerContext context, ListImportRowRequest request,
        CancellationToken cancellationToken = default)
    {
        var ids = new ImportDocumentAndUserIds(request.DocumentId, context.UserId);
        var documentResult = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!documentResult.Success)
            return documentResult.Error;

        var filter = new ImportRowByDocumentFilter(request.DocumentId)
        {
            Limit = (int)request.Limit,
            Offset = (int)request.Offset
        };

        var result = await _repository.FindRowsAsync(filter, cancellationToken);
        if (!result.Success)
            return result.Error;

        var rows = await result.Value.ToArrayAsync(cancellationToken);
        return new ListImportRowResponse(rows, result.Value.Count);
    }
}
