using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.Users.Password.Register.Models;

public sealed class RegisterUserPasswordResponse
{
    public UserIdentifier UserId { get; }
    
    public RegisterUserPasswordResponse(UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        UserId = userId;
    }
}