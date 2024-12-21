using System.Security.Cryptography;
using Freem.Crypto.Hashes.Abstractions;

namespace Freem.Crypto.Hashes.Implementations.Hashers;

public class Sha512RawHasher : RawHasher
{
    private readonly SHA512 _internalHasher;

    public Sha512RawHasher()
        : base("SHA512")
    {
        _internalHasher = SHA512.Create();
    }
    
    public override byte[] Hash(byte[] input)
    {
        return _internalHasher.ComputeHash(input);
    }
}