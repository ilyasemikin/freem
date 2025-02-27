using Freem.Entities.Users.Models;

namespace Freem.Web.Api.Public.Contracts.Users.LoginPassword;

public sealed class UpdatePasswordCredentialsRequest
{
    public Password OldPassword { get; }
    public Password NewPassword { get; }

    public UpdatePasswordCredentialsRequest(Password oldPassword, Password newPassword)
    {
        ArgumentNullException.ThrowIfNull(oldPassword);
        ArgumentNullException.ThrowIfNull(newPassword);
        
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }
}