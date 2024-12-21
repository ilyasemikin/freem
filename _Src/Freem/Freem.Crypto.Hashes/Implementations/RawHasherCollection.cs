using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using Freem.Crypto.Hashes.Abstractions;
using Freem.Crypto.Hashes.Abstractions.Models;

namespace Freem.Crypto.Hashes.Implementations;

public class RawHasherCollection
{
    private readonly FrozenDictionary<HashAlgorithm, RawHasher> _hashers;

    public RawHasherCollection(IEnumerable<RawHasher> hashers)
    {
        _hashers = hashers.ToFrozenDictionary(hasher => hasher.Algorithm);
    }

    public bool TryGet(HashAlgorithm algorithm, [NotNullWhen(true)] out RawHasher? hasher)
    {
        return _hashers.TryGetValue(algorithm, out hasher);
    }
}