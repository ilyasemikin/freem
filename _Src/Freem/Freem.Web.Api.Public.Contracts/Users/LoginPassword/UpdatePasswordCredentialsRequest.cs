using Freem.Entities.Models.Users;

namespace Freem.Web.Api.Public.Contracts.Users.LoginPassword;

public sealed class UpdatePasswordCredentialsRequest
{
    public Login Login { get; }
    public Password OldPassword { get; }
    public Password NewPassword { get; }

    public UpdatePasswordCredentialsRequest(Login login, Password oldPassword, Password newPassword)
    {
        ArgumentNullException.ThrowIfNull(login);
        ArgumentNullException.ThrowIfNull(oldPassword);
        ArgumentNullException.ThrowIfNull(newPassword);
        
        Login = login;
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }
}