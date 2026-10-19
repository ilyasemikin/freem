using Freem.Domain.Storage.Models.Identifiers;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Records.Get;

namespace Freem.Entities.UseCases.Records;

internal sealed class GetRecordUseCase 
    : IEntitiesUseCase<GetRecordRequest, GetRecordResponse, GetRecordErrorCode>
{
    private readonly IRecordRepository _repository;

    public GetRecordUseCase(IRecordRepository repository)
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