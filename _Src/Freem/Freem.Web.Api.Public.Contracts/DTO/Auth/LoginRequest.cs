namespace Freem.Web.Api.Public.Contracts.DTO.Auth;

public sealed class LoginRequest
{
    public string Username { get; }
    public string Password { get; }

    public LoginRequest(string username, string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        Username = username;
        Password = password;
    }
}
