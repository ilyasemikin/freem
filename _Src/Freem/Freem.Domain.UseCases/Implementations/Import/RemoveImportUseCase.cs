using Freem.Domain.Storage.Repositories.Imports;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Imports.Remove;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Import;

internal sealed class RemoveImportUseCase
    : IRequestHandler<HandlerContext, RemoveImportRequest, Result<Error>>
{
    private readonly IImportDocumentRepository _repository;

    public RemoveImportUseCase(IImportDocumentRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task<Result<Error>> HandleAsync(
        HandlerContext context, RemoveImportRequest request,
        CancellationToken cancellationToken = default)
    {
        var ids = new ImportDocumentAndUserIds(request.Id, context.UserId);
        var documentResult = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!documentResult.Success)
            return documentResult.Error;

        return await _repository.DeleteAsync(request.Id, cancellationToken);
    }
}
