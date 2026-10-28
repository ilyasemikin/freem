using Freem.Api.Public.Contracts.V1.Auth;
using Freem.Api.Public.Contracts.V1.Users;
using Freem.Domain.Users.Models;
using Freem.Api.Public.IntegrationTests.Extensions;
using Utilities.Http.Errors.Abstractions;
using Utilities.Primitives;

namespace Freem.Api.Public.IntegrationTests.Context.Preparers;

public sealed class UserPreparer
{
    public const string NicknameValue = "nickname";
    
    private readonly TestContext _context;

    public UserPreparer(TestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        _context = context;
    }

    // Creates a new EgoHub identity and starts a session; the user is not registered in Freem yet.
    public async Task AuthenticateAsync()
    {
        var username = $"it-{Guid.NewGuid():N}";
        var password = $"Aa1!{Guid.NewGuid():N}";

        var registerRequest = new RegisterRequest(username, password);
        var registerResponse = await WithRetryAsync(() => _context.Client.Auth.RegisterAsync(registerRequest));
        registerResponse.EnsureSuccess();

        var loginRequest = new LoginRequest(username, password);
        var loginResponse = await WithRetryAsync(() => _context.Client.Auth.LoginAsync(loginRequest));
        var session = loginResponse.Unwrap();

        _context.ClientSession.Set(session.AccessToken);
    }

    public async Task RegisterAsync()
    {
        await AuthenticateAsync();
        
        var nickname = Nickname.Parse(NicknameValue, null);
        var request = new UserRegisterRequest(nickname);
        var response = await _context.Client.Users.RegisterAsync(request);
        response.EnsureSuccess();
    }

    // Parallel tests sign up many EgoHub users at once, so a transient failure is retried with a growing delay.
    private static async Task<Result<T, IClientError>> WithRetryAsync<T>(Func<Task<Result<T, IClientError>>> action)
    {
        const int maxAttempts = 5;

        for (var attempt = 1; attempt < maxAttempts; attempt++)
        {
            var result = await action();
            if (result.Success)
                return result;

            var delay = TimeSpan.FromMilliseconds(200 * attempt);
            await Task.Delay(delay);
        }

        return await action();
    }
}
