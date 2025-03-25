using Freem.Web.Api.Public.Contracts.Users.LoginPassword;

namespace Freem.Web.Api.Public.FunctionalTests.Context.Preparers;

public sealed class UserPreparer
{
    public const string NicknameValue = "nickname";
    public const string LoginValue = "login";
    public const string PasswordValue = "password";
    
    private readonly TestContext _context;
    
    public UserPreparer(TestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        _context = context;
    }

    public void Register()
    {
        var request = new RegisterPasswordCredentialsRequest(NicknameValue, LoginValue, PasswordValue);
        var response = _context.SyncClient.Users.Register(request);
        response.EnsureSuccess();
    }

    public void Login()
    {
        var lrq = new LoginPasswordCredentialsRequest(LoginValue, PasswordValue);
        var lrs = _context.SyncClient.Users.Login(lrq);
        lrs.EnsureSuccess();
        
        _context.TokenLoader.Update(lrs.Value.Tokens);
    }
}