using Freem.Entities.Users.Models;

namespace Freem.Entities.Users;

public sealed class UserPasswordCredentials
{
    public Login Login { get; }
    public PasswordHash PasswordHash { get; }

    public UserPasswordCredentials(Login login, PasswordHash passwordHash)
    {
        ArgumentNullException.ThrowIfNull(login);
        ArgumentNullException.ThrowIfNull(passwordHash);

        Login = login;
        PasswordHash = passwordHash;
    }
}