using Freem.Entities.UseCases.Contracts.Users.Settings.Update;
using Freem.Entities.UseCases.Plain.Implementations.Executors.Async;
using Freem.Entities.Users;
using Freem.Results;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations.Executors.Sync;

public sealed class UsersSettingsPlainSyncExecutor
{
    private readonly UsersSettingsPlainExecutor _executor;

    public UsersSettingsPlainSyncExecutor(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        _executor = new UsersSettingsPlainExecutor(executor);
    }

    public void Update(UseCaseExecutionContext context, UpdateUserSettingsRequest request)
    {
        _executor.UpdateAsync(context, request)
            .GetAwaiter()
            .GetResult();
    }

    public Result<UserSettings> Get(UseCaseExecutionContext context)
    {
        return _executor.GetAsync(context)
            .GetAwaiter()
            .GetResult();
    }

    public UserSettings RequiredGet(UseCaseExecutionContext context)
    {
        return _executor.RequiredGetAsync(context)
            .GetAwaiter()
            .GetResult();
    }
}