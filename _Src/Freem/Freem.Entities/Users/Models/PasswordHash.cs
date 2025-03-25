using Freem.Crypto.Hashes.Abstractions.Models;

namespace Freem.Entities.Users.Models;

public sealed class PasswordHash : IEquatable<PasswordHash>
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
    
    public bool Equals(PasswordHash? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;

        if (!Algorithm.Equals(other.Algorithm))
            return false;
        if (_value.Length != other._value.Length)
            return false;
        if (_salt.Length != other._salt.Length)
            return false;

        for (var i = 0; i < _value.Length; i++)
            if (_value[i] != other._value[i])
                return false;
        
        for (var i = 0; i < _salt.Length; i++)
            if (_salt[i] != other._salt[i])
                return false;
        
        return false;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is PasswordHash other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_value, _salt, Algorithm);
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