using Freem.Domain.Users.Identifiers;
using Freem.Entities.UseCases.Contracts.Users.Register;
using Freem.Entities.UseCases.Plain.Implementations.Executors.Async;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations.Executors.Sync;

public sealed class UsersPlainSyncExecutor
{
    private readonly UsersPlainExecutor _executor;

    public UsersPlainSyncExecutor(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        _executor = new UsersPlainExecutor(executor);
    }

    public UserIdentifier Register(RegisterUserRequest request)
    {
        return _executor.RegisterAsync(request)
            .GetAwaiter()
            .GetResult();
    }

    public UserIdentifier Register()
    {
        return _executor.RegisterAsync()
            .GetAwaiter()
            .GetResult();
    }
}
