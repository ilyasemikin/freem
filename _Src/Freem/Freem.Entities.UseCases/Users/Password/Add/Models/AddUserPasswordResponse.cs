namespace Freem.Entities.UseCases.Users.Password.Add.Models;

public sealed class AddUserPasswordResponse
{
    public required bool Success { get; init; }

    public static AddUserPasswordResponse Added()
    {
        return new AddUserPasswordResponse
        {
            Success = true
        };
    }

    public static AddUserPasswordResponse Failed()
    {
        return new AddUserPasswordResponse
        {
            Success = false
        };
    }
}