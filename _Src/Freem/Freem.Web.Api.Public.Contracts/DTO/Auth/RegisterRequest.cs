namespace Freem.Web.Api.Public.Contracts.DTO.Auth;

public sealed class RegisterRequest
{
    public string Username { get; }
    public string Password { get; }

    public RegisterRequest(string username, string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        Username = username;
        Password = password;
    }
}
