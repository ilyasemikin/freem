using Freem.Domain.Entities.Import;
using Freem.Domain.Entities.Records;
using Freem.Domain.Import;
using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Storage.Repositories.Imports;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Imports.Apply;
using Freem.Domain.UseCases.Events;
using Freem.Domain.Users;
using Freem.Storage.Transactions.Helpers;
using Freem.Storage.Transactions.Helpers.Extensions;
using Utilities.Errors;
using Utilities.Locking;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Import;

internal sealed class ApplyImportUseCase
    : IRequestHandler<HandlerContext, ApplyImportRequest, Result<ApplyImportResponse, Error>>
{
    public const int MaxRowsCount = 50_000;

    private const int ReadBatchSize = Limit.MaxValue;
    private const int WriteBatchSize = 500;

    private readonly ILocker _locker;
    private readonly IImportDocumentRepository _repository;
    private readonly IRecordRepository _recordRepository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public ApplyImportUseCase(
        ILocker locker,
        IImportDocumentRepository repository,
        IRecordRepository recordRepository,
        EventProducer eventProducer,
        StorageTransactionRunner transactionRunner)
    {
        ArgumentNullException.ThrowIfNull(locker);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(recordRepository);
        ArgumentNullException.ThrowIfNull(eventProducer);
        ArgumentNullException.ThrowIfNull(transactionRunner);

        _locker = locker;
        _repository = repository;
        _recordRepository = recordRepository;
        _eventProducer = eventProducer;
        _transactionRunner = transactionRunner;
    }

    public async Task<Result<ApplyImportResponse, Error>> HandleAsync(
        HandlerContext context, ApplyImportRequest request,
        CancellationToken cancellationToken = default)
    {
        var lockResult = await _locker.LockAsync(Lock.Prefix + request.DocumentId, cancellationToken);
        if (!lockResult.Success)
            return lockResult.Error;

        await using var @lock = lockResult.Value;

        var ids = new ImportDocumentAndUserIds(request.DocumentId, context.UserId);
        var documentResult = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!documentResult.Success)
            return documentResult.Error;

        var document = documentResult.Value;
        var rowsResult = request.RowIds is null
            ? await ReadAllRowsAsync(document.Id, cancellationToken)
            : await ReadSelectedRowsAsync(document.Id, request.RowIds, cancellationToken);
        if (!rowsResult.Success)
            return rowsResult.Error;

        var activitiesResult = await ReadActivitiesAsync(document.Id, cancellationToken);
        if (!activitiesResult.Success)
            return activitiesResult.Error;

        var tagsResult = await ReadTagsAsync(document.Id, cancellationToken);
        if (!tagsResult.Success)
            return tagsResult.Error;

        var rows = rowsResult.Value;
        var activities = activitiesResult.Value;
        var tags = tagsResult.Value;
        var mappingCheck = new ImportMappingCheck(activities, tags);
        foreach (var row in rows)
            mappingCheck.Add(row);

        var unmapped = mappingCheck.FindUnmapped();
        if (unmapped.Count > 0)
            return DomainErrorCatalog.Factories.CreateImportUnmappedEntities(unmapped);

        return await _transactionRunner.RunAsync(
            () => WriteAsync(context.UserId, activities, tags, rows, cancellationToken),
            cancellationToken);
    }

    private async Task<Result<IReadOnlyList<ImportRow>, Error>> ReadAllRowsAsync(
        ImportDocumentId documentId,
        CancellationToken cancellationToken)
    {
        var rows = new List<ImportRow>();
        while (true)
        {
            var filter = new ImportRowByDocumentFilter(documentId)
            {
                Limit = ReadBatchSize,
                Offset = rows.Count
            };
            var result = await _repository.FindRowsAsync(filter, cancellationToken);
            if (!result.Success)
                return result.Error;

            if (result.Value.Count > MaxRowsCount)
                return DomainErrorCatalog.Factories.CreateImportTooManyRows(MaxRowsCount);

            var countBefore = rows.Count;
            await foreach (var row in result.Value.WithCancellation(cancellationToken))
                rows.Add(row);

            if (rows.Count - countBefore < ReadBatchSize)
                return rows;
        }
    }

    private async Task<Result<IReadOnlyList<ImportDocumentActivity>, Error>> ReadActivitiesAsync(
        ImportDocumentId documentId,
        CancellationToken cancellationToken)
    {
        var activities = new List<ImportDocumentActivity>();
        while (true)
        {
            var filter = new ImportDocumentActivityByDocumentFilter(documentId)
            {
                Limit = ReadBatchSize,
                Offset = activities.Count
            };
            var result = await _repository.FindActivitiesAsync(filter, cancellationToken);
            if (!result.Success)
                return result.Error;

            var countBefore = activities.Count;
            await foreach (var activity in result.Value.WithCancellation(cancellationToken))
                activities.Add(activity);

            if (activities.Count - countBefore < ReadBatchSize)
                return activities;
        }
    }

    private async Task<Result<IReadOnlyList<ImportDocumentTag>, Error>> ReadTagsAsync(
        ImportDocumentId documentId,
        CancellationToken cancellationToken)
    {
        var tags = new List<ImportDocumentTag>();
        while (true)
        {
            var filter = new ImportDocumentTagByDocumentFilter(documentId)
            {
                Limit = ReadBatchSize,
                Offset = tags.Count
            };
            var result = await _repository.FindTagsAsync(filter, cancellationToken);
            if (!result.Success)
                return result.Error;

            var countBefore = tags.Count;
            await foreach (var tag in result.Value.WithCancellation(cancellationToken))
                tags.Add(tag);

            if (tags.Count - countBefore < ReadBatchSize)
                return tags;
        }
    }

    private async Task<Result<IReadOnlyList<ImportRow>, Error>> ReadSelectedRowsAsync(
        ImportDocumentId documentId,
        IReadOnlyList<ImportRowId> rowIds,
        CancellationToken cancellationToken)
    {
        var distinctIds = rowIds
            .Distinct()
            .ToArray();
        if (distinctIds.Length == 0)
            return DomainErrorCatalog.Factories.CreateNothingToDo("No rows selected");
        if (distinctIds.Length > MaxRowsCount)
            return DomainErrorCatalog.Factories.CreateImportTooManyRows(MaxRowsCount);

        var rows = new List<ImportRow>();
        foreach (var chunk in distinctIds.Chunk(ReadBatchSize))
        {
            var filter = new ImportRowByDocumentFilter(documentId)
            {
                Limit = ReadBatchSize,
                Offset = 0,
                Ids = chunk
            };
            var result = await _repository.FindRowsAsync(filter, cancellationToken);
            if (!result.Success)
                return result.Error;

            await foreach (var row in result.Value.WithCancellation(cancellationToken))
                rows.Add(row);
        }

        if (rows.Count == distinctIds.Length)
            return rows;

        var foundIds = rows
            .Select(r => r.Id)
            .ToHashSet();
        var missingIds = distinctIds.Where(id => !foundIds.Contains(id));
        return DomainErrorCatalog.Factories.CreateRelatedEntitiesNotFound(missingIds);
    }

    private async Task<Result<ApplyImportResponse, Error>> WriteAsync(
        UserId userId,
        IReadOnlyList<ImportDocumentActivity> activities,
        IReadOnlyList<ImportDocumentTag> tags,
        IReadOnlyList<ImportRow> rows,
        CancellationToken cancellationToken)
    {
        var mappings = new ImportMappings(activities, tags);

        var records = new List<Record>();
        var skippedRowIds = new List<ImportRowId>();
        foreach (var row in rows)
        {
            var record = ImportRecordFactory.TryCreate(row, userId, mappings);
            if (record is null)
                skippedRowIds.Add(row.Id);
            else
                records.Add(record);
        }

        foreach (var chunk in records.Chunk(WriteBatchSize))
        {
            var error = await CreateRecordsAsync(userId, chunk, cancellationToken);
            if (error is not null)
                return error;
        }

        return new ApplyImportResponse(records.Count, skippedRowIds);
    }

    private async Task<Error?> CreateRecordsAsync(
        UserId userId,
        IReadOnlyList<Record> records,
        CancellationToken cancellationToken)
    {
        var createResult = await _recordRepository.CreateManyAsync(records, cancellationToken);
        if (!createResult.Success)
            return createResult.Error;

        var factories = records
            .Select(r => (EventProducer.EventFactory)r.BuildCreatedEvent)
            .ToArray();
        var publishResult = await _eventProducer.PublishManyAsync(userId, factories, cancellationToken);
        return publishResult.Success
            ? null
            : publishResult.Error;
    }
}
