using Freem.Domain.Storage.Repositories.DataTransfer.Import;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.ListTags;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Import;

internal sealed class ListImportTagUseCase
    : IRequestHandler<HandlerContext, ListImportTagRequest, Result<ListImportTagResponse, Error>>
{
    private readonly IImportDocumentRepository _repository;

    public ListImportTagUseCase(IImportDocumentRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task<Result<ListImportTagResponse, Error>> HandleAsync(
        HandlerContext context, ListImportTagRequest request,
        CancellationToken cancellationToken = default)
    {
        var ids = new ImportDocumentAndUserIds(request.DocumentId, context.UserId);
        var documentResult = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!documentResult.Success)
            return documentResult.Error;

        var filter = new ImportDocumentTagByDocumentFilter(request.DocumentId)
        {
            Limit = (int)request.Limit,
            Offset = (int)request.Offset
        };
        var result = await _repository.FindTagsAsync(filter, cancellationToken);
        if (!result.Success)
            return result.Error;

        var tags = await result.Value.ToArrayAsync(cancellationToken);
        return new ListImportTagResponse(tags, result.Value.Count);
    }
}
