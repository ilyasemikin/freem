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

    public void Register()
    {
        var request = new RegisterUserRequest(NicknameValue);
        var response = _context.SyncClient.Users.Register(request);
        response.EnsureSuccess();
        
        _context.CurrentUser.Set(response.Value.UserId);
    }
}
