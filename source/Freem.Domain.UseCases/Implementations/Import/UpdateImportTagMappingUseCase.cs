using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Imports;
using Freem.Domain.Storage.Repositories.Tags;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Imports.UpdateTagMapping;
using Utilities.Errors;
using Utilities.Locking;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Import;

internal sealed class UpdateImportTagMappingUseCase
    : IRequestHandler<HandlerContext, UpdateImportTagMappingRequest, Result<Error>>
{
    private readonly ILocker _locker;
    private readonly IImportDocumentRepository _repository;
    private readonly ITagRepository _tagRepository;

    public UpdateImportTagMappingUseCase(
        ILocker locker,
        IImportDocumentRepository repository,
        ITagRepository tagRepository)
    {
        ArgumentNullException.ThrowIfNull(locker);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(tagRepository);

        _locker = locker;
        _repository = repository;
        _tagRepository = tagRepository;
    }

    public async Task<Result<Error>> HandleAsync(
        HandlerContext context, UpdateImportTagMappingRequest request,
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

        var error = await ValidateAsync(context, request, cancellationToken);
        if (error is not null)
            return error;

        return await _repository.UpdateTagMappingAsync(
            request.DocumentId, request.ImportTagId, request.TagId, cancellationToken);
    }

    private async Task<Error?> ValidateAsync(
        HandlerContext context, UpdateImportTagMappingRequest request,
        CancellationToken cancellationToken)
    {
        if (request.TagId is null)
            return null;

        var tagIds = new TagAndUserIds(request.TagId, context.UserId);
        var tagResult = await _tagRepository.FindByMultipleIdAsync(tagIds, cancellationToken);
        return tagResult.Success
            ? null
            : tagResult.Error;
    }
}
