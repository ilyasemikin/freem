using Freem.Domain.Users.Models;
using Freem.Api.Public.Contracts.DTO.Auth;
using Freem.Api.Public.Contracts.DTO.Users;
using Freem.Api.Public.IntegrationTests.Extensions;

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
        var registerResponse = await _context.Client.Auth.RegisterAsync(registerRequest);
        registerResponse.EnsureSuccess();

        var loginRequest = new LoginRequest(username, password);
        var loginResponse = await _context.Client.Auth.LoginAsync(loginRequest);
        var session = loginResponse.Unwrap();

        _context.ClientSession.Set(session.AccessToken);
    }

    public async Task RegisterAsync()
    {
        await AuthenticateAsync();
        
        var nickname = Nickname.Parse(NicknameValue, null);
        var request = new RegisterUserRequest(nickname);
        var response = await _context.Client.Users.RegisterAsync(request);
        response.EnsureSuccess();
    }
}
