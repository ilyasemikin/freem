using Freem.Domain.Storage.Repositories.Users;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Users.Get;

namespace Freem.Entities.UseCases.Users;

internal sealed class GetUserUseCase : IEntitiesUseCase<GetUserRequest, GetUserResponse, GetUserErrorCode>
{
    private readonly IUserRepository _repository;

    public GetUserUseCase(IUserRepository repository)
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