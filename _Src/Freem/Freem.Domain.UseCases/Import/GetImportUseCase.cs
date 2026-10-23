using Freem.Domain.Storage.Repositories.Imports;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Imports.Get;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Import;

internal sealed class GetImportUseCase
    : IRequestHandler<HandlerContext, GetImportRequest, Result<GetImportResponse, Error>>
{
    private readonly IImportDocumentRepository _repository;

    public GetImportUseCase(IImportDocumentRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task<Result<GetImportResponse, Error>> HandleAsync(
        HandlerContext context, GetImportRequest request,
        CancellationToken cancellationToken = default)
    {
        var ids = new ImportDocumentAndUserIds(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Success)
            return result.Error;

        return new GetImportResponse(result.Value);
    }
}
