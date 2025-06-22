using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Users.Get;
using Freem.Entities.Users;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.Users;

internal sealed class GetUserUseCase : IEntitiesUseCase<GetUserRequest, GetUserResponse, GetUserErrorCode>
{
    private readonly ISearchByIdRepository<User, UserIdentifier> _repository;

    public GetUserUseCase(ISearchByIdRepository<User, UserIdentifier> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task<GetUserResponse> ExecuteAsync(
        UseCaseExecutionContext context,
        GetUserRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        var result = await _repository.FindByIdAsync(context.UserId, cancellationToken);
        return result.Founded 
            ? GetUserResponse.CreateSuccess(result.Entity) 
            : GetUserResponse.CreateNotFoundResult();
    }
}