using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.ListByPeriod;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Records;

internal sealed class ListRecordByPeriodUseCase : 
    IRequestHandler<HandlerContext, ListRecordByPeriodRequest, Result<ListRecordByPeriodResponse, Error>>
{
    private readonly IRecordRepository _repository;

    public ListRecordByPeriodUseCase(
        IRecordRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<Result<ListRecordByPeriodResponse, Error>> HandleAsync(
        HandlerContext context, ListRecordByPeriodRequest request,
        CancellationToken cancellationToken = default)
    {
        var filter = new RecordByPeriodFilter(context.UserId, request.Period);
        
        var result = await _repository.FindAsync(filter, cancellationToken);
        if (!result.Success)
            return result.Error;

        var records = await result.Value.ToArrayAsync(cancellationToken);
        return new ListRecordByPeriodResponse(records, result.Value.Count);
    }
}