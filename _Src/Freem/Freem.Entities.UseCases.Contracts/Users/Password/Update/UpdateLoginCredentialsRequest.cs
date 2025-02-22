namespace Freem.Entities.UseCases.Contracts.Users.Password.Update;

public class UpdateLoginCredentialsRequest
{
    public Models.Users.Password OldPassword { get; }
    public Models.Users.Password NewPassword { get; }
    
    public UpdateLoginCredentialsRequest(
        Models.Users.Password oldPassword, 
        Models.Users.Password newPassword)
    {
        ArgumentNullException.ThrowIfNull(oldPassword);
        ArgumentNullException.ThrowIfNull(newPassword);
        
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }
}