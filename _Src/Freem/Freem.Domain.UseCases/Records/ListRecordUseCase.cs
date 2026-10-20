using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.List;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Records;

internal sealed class ListRecordUseCase 
    : IRequestHandler<HandlerContext, ListRecordRequest, Result<ListRecordResponse, Error>>
{
    private readonly IRecordRepository _repository;

    public ListRecordUseCase(
        IRecordRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<Result<ListRecordResponse, Error>> HandleAsync(
        HandlerContext context, ListRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        var filter = new RecordByUserFilter(context.UserId)
        {
            Limit = (int)request.Limit,
            Offset = (int)request.Offset
        };
        
        var result = await _repository.FindAsync(filter, cancellationToken);
        if (!result.Success)
            return result.Error;

        var records = await result.Value.ToArrayAsync(cancellationToken);
        return new ListRecordResponse(records, result.Value.Count);
    }
}