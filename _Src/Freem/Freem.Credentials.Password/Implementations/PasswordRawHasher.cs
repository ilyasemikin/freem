using Freem.Crypto.Hashes.Abstractions.Models;
using Freem.Crypto.Hashes.Implementations;

namespace Freem.Credentials.Password.Implementations;

public sealed class PasswordRawHasher
{
    private readonly RawHasherCollection _rawHasherCollection;

    public PasswordRawHasher(RawHasherCollection rawHasherCollection)
    {
        ArgumentNullException.ThrowIfNull(rawHasherCollection);
        
        _rawHasherCollection = rawHasherCollection;
    }

    public byte[] Hash(HashAlgorithm algorithm, IReadOnlyList<byte> password, IReadOnlyList<byte> salt)
    {
        if (!_rawHasherCollection.TryGet(algorithm, out var hasher))
            throw new InvalidOperationException($"The hash algorithm \"{algorithm}\" is not supported.");

        var bytes = new byte[password.Count + salt.Count];
        for (var i = 0; i < password.Count; i++)
            bytes[i] = password[i];
        for (var i = 0; i < salt.Count; i++)
            bytes[password.Count + i] = salt[i];
        
        return hasher.Hash(bytes);
    }
}