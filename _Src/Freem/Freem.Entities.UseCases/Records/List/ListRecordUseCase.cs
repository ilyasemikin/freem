﻿using Freem.Entities.Records;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Records.List.Models;

namespace Freem.Entities.UseCases.Records.List;

internal class ListRecordUseCase : IUseCase<ListRecordRequest, ListRecordResponse>
{
    private readonly IMultipleSearchByFilterRepository<Record, RecordIdentifier, RecordsByUserFilter> _repository;

    public ListRecordUseCase(
        IMultipleSearchByFilterRepository<Record, RecordIdentifier, RecordsByUserFilter> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<ListRecordResponse> ExecuteAsync(
        UseCaseExecutionContext context, ListRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var filter = Map();
        var result = await _repository.FindAsync(filter, cancellationToken);
        return new ListRecordResponse(result, result.TotalCount);
        
        RecordsByUserFilter Map()
        {
            return new RecordsByUserFilter(context.UserId)
            {
                Limit = request.Limit,
                Offset = request.Offset
            };
        }
    }
}