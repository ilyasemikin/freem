namespace Freem.Crypto.Hashes.Abstractions.Models;

public sealed class HashAlgorithm
{
    private readonly string _name;
    
    public HashAlgorithm(string name)
    {
        _name = name;
    }

    public override string ToString()
    {
        return _name;
    }

    public override int GetHashCode()
    {
        return _name.GetHashCode();
    }
    
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is HashAlgorithm other && _name == other._name;
    }

    public static implicit operator string(HashAlgorithm algorithm)
    {
        return algorithm._name;
    }
}