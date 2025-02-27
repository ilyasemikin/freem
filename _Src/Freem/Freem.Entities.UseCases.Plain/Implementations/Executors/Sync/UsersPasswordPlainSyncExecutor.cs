using Freem.Entities.UseCases.Contracts.Users.Password.Login;
using Freem.Entities.UseCases.Contracts.Users.Password.Register;
using Freem.Entities.UseCases.Contracts.Users.Password.Update;
using Freem.Entities.UseCases.Plain.Implementations.Executors.Async;
using Freem.Entities.Users.Identifiers;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations.Executors.Sync;

public sealed class UsersPasswordPlainSyncExecutor
{
    private readonly UsersPasswordPlainExecutor _executor;

    public UsersPasswordPlainSyncExecutor(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        _executor = new UsersPasswordPlainExecutor(executor);
    }

    public UserIdentifier Register(RegisterUserPasswordRequest request)
    {
        return _executor.RegisterAsync(request)
            .GetAwaiter()
            .GetResult();
    }

    public UserIdentifier Register(string? login = null)
    {
        return _executor.RegisterAsync(login)
            .GetAwaiter()
            .GetResult();
    }

    public UsersPasswordPlainExecutor.Tokens Login(LoginUserPasswordRequest request)
    {
        return _executor.LoginAsync(request)
            .GetAwaiter()
            .GetResult();
    }

    public void Update(UseCaseExecutionContext context, UpdateLoginCredentialsRequest request)
    {
        _executor.UpdateAsync(context, request)
            .GetAwaiter()
            .GetResult();
    }
}