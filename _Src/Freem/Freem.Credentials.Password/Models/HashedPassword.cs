using Freem.Crypto.Hashes.Abstractions.Models;

namespace Freem.Credentials.Password.Models;

public class HashedPassword
{
    public HashAlgorithm Algorithm { get; }
    
    public string Password { get; }
    public string Salt { get; }

    public HashedPassword(HashAlgorithm algorithm, byte[] password, byte[] salt)
    {
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(password);
        ArgumentNullException.ThrowIfNull(salt);
        
        Algorithm = algorithm;
        Password = Convert.ToBase64String(password);
        Salt = Convert.ToBase64String(salt);
    }
}