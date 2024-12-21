using Freem.Entities.Users.Models;

namespace Freem.Entities.Users;

public sealed class UserPasswordCredentials
{
    public string Login { get; }
    public PasswordHash PasswordHash { get; }

    public UserPasswordCredentials(string login, PasswordHash passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(login);
        ArgumentNullException.ThrowIfNull(passwordHash);

        Login = login;
        PasswordHash = passwordHash;
    }
}