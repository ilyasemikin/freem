using Freem.Crypto.Hashes.Abstractions.Models;

namespace Freem.Credentials.Password.Abstractions;

public interface ICurrentPasswordHashAlgorithmGetter
{
    Task<HashAlgorithm> GetAsync(CancellationToken cancellationToken = default);
}