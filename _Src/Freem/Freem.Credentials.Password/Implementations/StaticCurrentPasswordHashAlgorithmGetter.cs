using Freem.Credentials.Password.Abstractions;
using Freem.Crypto.Hashes.Abstractions.Models;

namespace Freem.Credentials.Password.Implementations;

public sealed class StaticCurrentPasswordHashAlgorithmGetter : ICurrentPasswordHashAlgorithmGetter
{
    private readonly HashAlgorithm _algorithm;

    public StaticCurrentPasswordHashAlgorithmGetter(HashAlgorithm algorithm)
    {
        ArgumentNullException.ThrowIfNull(algorithm);
        
        _algorithm = algorithm;
    }

    public async Task<HashAlgorithm> GetAsync(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_algorithm);
    }
}