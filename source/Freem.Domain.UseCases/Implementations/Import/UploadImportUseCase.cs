using Freem.Domain.Entities.Import;
using Freem.Domain.Storage.Repositories.Imports;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Imports.Upload;
using Freem.Domain.UseCases.Implementations.Users.Settings;
using Freem.Storage.Transactions.Implementations;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Import;

internal sealed class UploadImportUseCase
    : IRequestHandler<HandlerContext, UploadImportRequest, Result<UploadImportResponse, Error>>
{
    private const int RowsBatchSize = 500;

    private readonly ImportParserRegistry _parsers;
    private readonly IImportDocumentRepository _repository;
    private readonly StorageTransactionRunner _transactionRunner;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public UploadImportUseCase(
        ImportParserRegistry parsers,
        IImportDocumentRepository repository,
        StorageTransactionRunner transactionRunner,
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(parsers);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(transactionRunner);
        ArgumentNullException.ThrowIfNull(mediator);

        _parsers = parsers;
        _repository = repository;
        _transactionRunner = transactionRunner;
        _mediator = mediator;
    }

    public async Task<Result<UploadImportResponse, Error>> HandleAsync(
        HandlerContext context, UploadImportRequest request,
        CancellationToken cancellationToken = default)
    {
        var parserResult = _parsers.Find(request.Format);
        if (!parserResult.Success)
            return parserResult.Error;

        var timeZoneOffset = request.TimeZoneOffset;
        if (timeZoneOffset is null)
        {
            var settingsResult = await _mediator.GetUserSettingsAsync(context, cancellationToken);
            if (!settingsResult.Success)
                return settingsResult.Error;

            timeZoneOffset = settingsResult.Value.TimeZoneOffset;
        }

        var options = new ImportParseOptions(timeZoneOffset);
        var parseResult = await parserResult.Value.ParseAsync(request.Content, options, cancellationToken);
        if (!parseResult.Success)
            return parseResult.Error;

        var parsed = parseResult.Value;
        var document = ImportDocumentFactory.CreateDocument(
            parsed,
            context.UserId,
            request.Format,
            request.FileName,
            timeZoneOffset);
        var tags = ImportDocumentFactory.CreateTags(parsed);
        var activities = ImportDocumentFactory.CreateActivities(parsed);
        var rows = ImportDocumentFactory.CreateRows(document.Id, parsed);

        var writeResult = await _transactionRunner.RunAsync(
            _ => WriteAsync(document, tags, activities, rows, cancellationToken),
            cancellationToken);
        if (!writeResult.Success)
            return writeResult.Error;

        return new UploadImportResponse(document, parsed.Warnings);
    }

    private async Task<Result<Error>> WriteAsync(
        ImportDocument document,
        IReadOnlyList<ImportDocumentTag> tags,
        IReadOnlyList<ImportDocumentActivity> activities,
        IReadOnlyList<ImportRow> rows,
        CancellationToken cancellationToken)
    {
        var createResult = await _repository.CreateAsync(document, cancellationToken);
        if (!createResult.Success)
            return createResult;

        var addResult = await _repository.AddTagsAndActivitiesAsync(document.Id, tags, activities, cancellationToken);
        if (!addResult.Success)
            return addResult;

        foreach (var batch in rows.Chunk(RowsBatchSize))
        {
            var addRowsResult = await _repository.AddRowsAsync(batch, cancellationToken);
            if (!addRowsResult.Success)
                return addRowsResult;
        }

        return Result<Error>.CreateSuccess();
    }
}
