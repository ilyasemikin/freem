using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
using Freem.Entities.UseCases.Contracts.Users.Settings.Update;
using Freem.Entities.UseCases.Plain.Exceptions;
using Freem.Entities.Users;
using Freem.Results;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations.Executors.Async;

public sealed class UsersSettingsPlainExecutor
{
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public UsersSettingsPlainExecutor(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        _executor = executor;
    }

    public async Task UpdateAsync(
        UseCaseExecutionContext context, UpdateUserSettingsRequest request, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);

        var response = await _executor.ExecuteAsync<UpdateUserSettingsRequest, UpdateUserSettingsResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();
    }

    public async Task<Result<UserSettings>> GetAsync(
        UseCaseExecutionContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        var request = new GetUserSettingsRequest();
        var response = await _executor.ExecuteAsync<GetUserSettingsRequest, GetUserSettingsResponse>(context, request, cancellationToken);
        return response.Success
            ? Result<UserSettings>.CreateSuccess(response.Settings)
            : Result<UserSettings>.CreateFailure();
    }

    public async Task<UserSettings> RequiredGetAsync(
        UseCaseExecutionContext context, CancellationToken cancellationToken = default)
    {
        var result = await GetAsync(context, cancellationToken);
        if (!result.Success)
            throw new PlainRequestFailedException();

        return result.Value;
    }
}