using Freem.Domain.Storage.Repositories.Imports;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Imports.List;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Import;

internal sealed class ListImportUseCase
    : IRequestHandler<HandlerContext, ListImportRequest, Result<ListImportResponse, Error>>
{
    private readonly IImportDocumentRepository _repository;

    public ListImportUseCase(IImportDocumentRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task<Result<ListImportResponse, Error>> HandleAsync(
        HandlerContext context, ListImportRequest request,
        CancellationToken cancellationToken = default)
    {
        var filter = new ImportDocumentByUserFilter(context.UserId)
        {
            Limit = (int)request.Limit,
            Offset = (int)request.Offset
        };

        var result = await _repository.FindAsync(filter, cancellationToken);
        if (!result.Success)
            return result.Error;

        var documents = await result.Value.ToArrayAsync(cancellationToken);
        return new ListImportResponse(documents, result.Value.Count);
    }
}
