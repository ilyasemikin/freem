namespace Freem.Api.Public.Contracts.DTO.Auth;

/// <summary>Credentials for a new identity.</summary>
public sealed class RegisterRequest
{
    /// <summary>Username. Must not be blank.</summary>
    public string Username { get; }
    /// <summary>Password. Must not be blank.</summary>
    public string Password { get; }

    public RegisterRequest(string username, string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        Username = username;
        Password = password;
    }
}
