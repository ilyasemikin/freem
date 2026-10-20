using Freem.Domain;
using Freem.Domain.Storage.Repositories.Users;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Users.Settings;

internal sealed class GetUserSettingsUseCase : 
    IRequestHandler<HandlerContext, GetUserSettingsRequest, Result<GetUserSettingsResponse, Error>>
{
    private readonly IUserRepository _repository;

    public GetUserSettingsUseCase(IUserRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<Result<GetUserSettingsResponse, Error>> HandleAsync(
        HandlerContext context, GetUserSettingsRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.FindByIdAsync(context.UserId, cancellationToken);
        if (!result.Success)
        {
            return result.Error.Type == DomainErrorCatalog.Types.EntityNotFound
                ? DomainErrorCatalog.Factories.CreateUserNotRegistered(context.UserId)
                : result.Error;
        }
        
        var user = result.Value;
        return new GetUserSettingsResponse(user.Settings);
    }
}