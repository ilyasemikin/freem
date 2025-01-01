namespace Freem.Entities.UseCases.Users.Password.Update.Models;

public sealed class UpdateLoginCredentialsResponse
{
    public required bool Success { get; init; }

    public static UpdateLoginCredentialsResponse Updated()
    {
        return new UpdateLoginCredentialsResponse
        {
            Success = true
        };
    }
    
    public static UpdateLoginCredentialsResponse Failure()
    {
        return new UpdateLoginCredentialsResponse
        {
            Success = false
        };
    }
}