using Freem.Domain.Storage.Repositories.DataTransfer.Import;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.ListActivities;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Import;

internal sealed class ListImportActivityUseCase
    : IRequestHandler<HandlerContext, ListImportActivityRequest, Result<ListImportActivityResponse, Error>>
{
    private readonly IImportDocumentRepository _repository;

    public ListImportActivityUseCase(IImportDocumentRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task<Result<ListImportActivityResponse, Error>> HandleAsync(
        HandlerContext context, ListImportActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        var ids = new ImportDocumentAndUserIds(request.DocumentId, context.UserId);
        var documentResult = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!documentResult.Success)
            return documentResult.Error;

        var filter = new ImportDocumentActivityByDocumentFilter(request.DocumentId)
        {
            Limit = (int)request.Limit,
            Offset = (int)request.Offset
        };
        var result = await _repository.FindActivitiesAsync(filter, cancellationToken);
        if (!result.Success)
            return result.Error;

        var activities = await result.Value.ToArrayAsync(cancellationToken);
        return new ListImportActivityResponse(activities, result.Value.Count);
    }
}
