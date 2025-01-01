using Freem.Crypto.Hashes.Abstractions.Models;

namespace Freem.Entities.Users.Models;

public sealed class PasswordHash
{
    private readonly byte[] _value;
    private readonly byte[] _salt;
    
    public HashAlgorithm Algorithm { get; }
    
    public IReadOnlyList<byte> Value => _value;
    public IReadOnlyList<byte> Salt => _salt;

    public string ValueBase64 => Convert.ToBase64String(_value);
    public string SaltBase64 => Convert.ToBase64String(_salt);
    
    public PasswordHash(HashAlgorithm algorithm, byte[] value, byte[] salt)
    {
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(salt);
        
        ArgumentOutOfRangeException.ThrowIfZero(value.Length);
        ArgumentOutOfRangeException.ThrowIfZero(salt.Length);
        
        Algorithm = algorithm;
        _value = value;
        _salt = salt;
    }

    public PasswordHash(HashAlgorithm algorithm, string valueBase64, string saltBase64)
    {
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentException.ThrowIfNullOrEmpty(valueBase64);
        ArgumentException.ThrowIfNullOrEmpty(saltBase64);
        
        Algorithm = algorithm;
        _value = Convert.FromBase64String(valueBase64);
        _salt = Convert.FromBase64String(saltBase64);
    }

    public static bool operator ==(PasswordHash hash, IReadOnlyList<byte> bytes)
    {
        if (hash.Value.Count != bytes.Count)
            return false;

        for (var i = 0; i < bytes.Count; i++)
            if (hash.Value[i] != bytes[i])
                return false;
        
        return true;
    }

    public static bool operator ==(IReadOnlyList<byte> bytes, PasswordHash hash)
    {
        return hash == bytes;
    }

    public static bool operator !=(PasswordHash hash, IReadOnlyList<byte> bytes)
    {
        return !(hash == bytes);
    }

    public static bool operator !=(IReadOnlyList<byte> bytes, PasswordHash hash)
    {
        return !(hash == bytes);
    }
}