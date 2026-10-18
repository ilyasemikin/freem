using Freem.Entities.UseCases.Contracts.Users.Register;
using Freem.Entities.UseCases.Plain.Exceptions;
using Freem.Entities.Users.Identifiers;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations.Executors.Async;

public sealed class UsersPlainExecutor
{
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public UsersPlainExecutor(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        _executor = executor;
    }

    public async Task<UserIdentifier> RegisterAsync(
        RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var response = await _executor.ExecuteAsync<RegisterUserRequest, RegisterUserResponse>(UseCaseExecutionContext.Empty, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();

        return response.UserId;
    }

    public async Task<UserIdentifier> RegisterAsync(CancellationToken cancellationToken = default)
    {
        var nickname = Guid.NewGuid().ToString();
        
        var request = new RegisterUserRequest(nickname);
        return await RegisterAsync(request, cancellationToken);
    }
}
