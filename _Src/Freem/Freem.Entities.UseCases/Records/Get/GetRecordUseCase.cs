using Freem.Entities.Records;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Records.Get.Models;

namespace Freem.Entities.UseCases.Records.Get;

internal sealed class GetRecordUseCase : IUseCase<GetRecordRequest, GetRecordResponse>
{
    private readonly ISearchByIdRepository<Record, RecordIdentifier> _repository;

    public GetRecordUseCase(ISearchByIdRepository<Record, RecordIdentifier> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<GetRecordResponse> ExecuteAsync(
        UseCaseExecutionContext context, GetRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        var ids = new RecordAndUserIdentifiers(request.Id, context.UserId);
        var result = await _repository.FindByIdAsync(request.Id, cancellationToken);
        return new GetRecordResponse(result.Entity);
    }
}