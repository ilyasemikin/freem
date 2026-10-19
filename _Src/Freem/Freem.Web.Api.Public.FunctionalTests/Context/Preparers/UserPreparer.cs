using Freem.Domain.Users.Models;
using Freem.Web.Api.Public.Contracts.DTO.Auth;
using Freem.Web.Api.Public.Contracts.DTO.Users;

namespace Freem.Web.Api.Public.FunctionalTests.Context.Preparers;

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
    public void Authenticate()
    {
        var username = $"it-{Guid.NewGuid():N}";
        var password = $"Aa1!{Guid.NewGuid():N}";

        var registerRequest = new RegisterRequest(username, password);
        var registerResponse = _context.SyncClient.Auth.Register(registerRequest);
        registerResponse.EnsureSuccess();

        var loginRequest = new LoginRequest(username, password);
        var loginResponse = _context.SyncClient.Auth.Login(loginRequest);
        loginResponse.EnsureSuccess();

        _context.ClientSession.Set(loginResponse.Value.AccessToken);
    }

    public void Register()
    {
        Authenticate();

        var nickname = Nickname.Parse(NicknameValue, null);
        var request = new RegisterUserRequest(nickname);
        var response = _context.SyncClient.Users.Register(request);
        response.EnsureSuccess();
    }
}
