using Freem.Entities.Records;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.DTO.Records.Get;

namespace Freem.Entities.UseCases.Records;

internal sealed class GetRecordUseCase : IUseCase<GetRecordRequest, GetRecordResponse, GetRecordErrorCode>
{
    private readonly ISearchByMultipleIdsRepository<Record, RecordIdentifier, RecordAndUserIdentifiers> _repository;

    public GetRecordUseCase(ISearchByMultipleIdsRepository<Record, RecordIdentifier, RecordAndUserIdentifiers> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<GetRecordResponse> ExecuteAsync(
        UseCaseExecutionContext context, GetRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var ids = new RecordAndUserIdentifiers(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        return GetRecordResponse.Create(result);
    }
}