using Freem.Domain.Storage.Repositories.Users;
using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Users.Settings;

internal sealed class GetUserSettingsUseCase : 
    IUseCase<UseCaseExecutionContext, GetUserSettingsRequest, GetUserSettingsResponse, GetUserSettingsErrorCode>
{
    private readonly IUserRepository _repository;

    public GetUserSettingsUseCase(IUserRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<GetUserSettingsResponse> ExecuteAsync(
        UseCaseExecutionContext context, GetUserSettingsRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        var result = await _repository.FindByIdAsync(context.UserId, cancellationToken);
        if (!result.Founded)
            return GetUserSettingsResponse.CreateFailure(GetUserSettingsErrorCode.UserNotFound);
        
        var user = result.Entity;
        return GetUserSettingsResponse.CreateSuccess(user.Settings);
    }
}