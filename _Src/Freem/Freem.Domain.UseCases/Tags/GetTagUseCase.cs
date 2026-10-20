using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Tags;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Tags.Get;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Tags;

internal sealed class GetTagUseCase 
    : IRequestHandler<HandlerContext, GetTagRequest, Result<GetTagResponse, Error>>
{
    private readonly ITagRepository _repository;

    public GetTagUseCase(ITagRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<Result<GetTagResponse, Error>> HandleAsync(
        HandlerContext context, GetTagRequest request,
        CancellationToken cancellationToken = default)
    {
        var ids = new TagAndUserIds(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Success)
            return result.Error;

        return new GetTagResponse(result.Value);
    }
}