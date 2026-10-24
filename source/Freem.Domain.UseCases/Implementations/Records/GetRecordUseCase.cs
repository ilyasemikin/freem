using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.Get;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Records;

internal sealed class GetRecordUseCase 
    : IRequestHandler<HandlerContext, GetRecordRequest, Result<GetRecordResponse, Error>>
{
    private readonly IRecordRepository _repository;

    public GetRecordUseCase(IRecordRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<Result<GetRecordResponse, Error>> HandleAsync(
        HandlerContext context, GetRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        var ids = new RecordAndUserIds(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Success)
            return result.Error;

        return new GetRecordResponse(result.Value);
    }
}