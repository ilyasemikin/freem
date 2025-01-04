namespace Freem.Entities.UseCases.Users.Password.Update.Models;

public class UpdateLoginCredentialsRequest
{
    public Entities.Users.Models.Password OldPassword { get; }
    public Entities.Users.Models.Password NewPassword { get; }
    
    public UpdateLoginCredentialsRequest(
        Entities.Users.Models.Password oldPassword, 
        Entities.Users.Models.Password newPassword)
    {
        ArgumentNullException.ThrowIfNull(oldPassword);
        ArgumentNullException.ThrowIfNull(newPassword);
        
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }
}