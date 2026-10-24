using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Domain.Storage.Repositories.Imports;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Imports.UpdateActivityMapping;
using Utilities.Errors;
using Utilities.Locking;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Import;

internal sealed class UpdateImportActivityMappingUseCase
    : IRequestHandler<HandlerContext, UpdateImportActivityMappingRequest, Result<Error>>
{
    private readonly ILocker _locker;
    private readonly IImportDocumentRepository _repository;
    private readonly IActivityRepository _activityRepository;

    public UpdateImportActivityMappingUseCase(
        ILocker locker,
        IImportDocumentRepository repository,
        IActivityRepository activityRepository)
    {
        ArgumentNullException.ThrowIfNull(locker);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(activityRepository);

        _locker = locker;
        _repository = repository;
        _activityRepository = activityRepository;
    }

    public async Task<Result<Error>> HandleAsync(
        HandlerContext context, UpdateImportActivityMappingRequest request,
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

        return await _repository.UpdateActivityMappingAsync(
            request.DocumentId, request.ImportActivityId, request.ActivityId, cancellationToken);
    }

    private async Task<Error?> ValidateAsync(
        HandlerContext context, UpdateImportActivityMappingRequest request,
        CancellationToken cancellationToken)
    {
        if (request.ActivityId is null)
            return null;

        var activityIds = new ActivityAndUserIds(request.ActivityId, context.UserId);
        var activityResult = await _activityRepository.FindByMultipleIdAsync(activityIds, cancellationToken);
        return activityResult.Success
            ? null
            : activityResult.Error;
    }
}
