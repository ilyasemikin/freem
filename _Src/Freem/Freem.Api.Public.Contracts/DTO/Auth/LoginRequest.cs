namespace Freem.Api.Public.Contracts.DTO.Auth;

/// <summary>Credentials for signing in.</summary>
public sealed class LoginRequest
{
    /// <summary>Username. Must not be blank.</summary>
    public string Username { get; }
    /// <summary>Password. Must not be blank.</summary>
    public string Password { get; }

    public LoginRequest(string username, string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        Username = username;
        Password = password;
    }
}
