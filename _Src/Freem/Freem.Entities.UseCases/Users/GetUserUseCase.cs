using Freem.Domain;
using Freem.Domain.Storage.Repositories.Users;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Users.Get;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Users;

internal sealed class GetUserUseCase : IRequestHandler<HandlerContext, GetUserRequest, Result<GetUserResponse, Error>>
{
    private readonly IUserRepository _repository;

    public GetUserUseCase(IUserRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task<Result<GetUserResponse, Error>> HandleAsync(
        HandlerContext context,
        GetUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.FindByIdAsync(context.UserId, cancellationToken);
        if (!result.Success)
        {
            return result.Error.Type == DomainErrorCatalog.Types.EntityNotFound
                ? DomainErrorCatalog.Factories.CreateUserNotRegistered(context.UserId)
                : result.Error;
        }

        return new GetUserResponse(result.Value);
    }
}