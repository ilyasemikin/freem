using Freem.Crypto.Hashes.Abstractions.Models;

namespace Freem.Crypto.Hashes.Abstractions;

public abstract class RawHasher
{
    public HashAlgorithm Algorithm { get; }
    
    protected internal RawHasher(string name)
    {
        Algorithm = new HashAlgorithm(name);
    }
    
    public abstract byte[] Hash(byte[] input);
}