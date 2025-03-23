using Freem.Crypto.Hashes.Abstractions.Models;

namespace Freem.Web.Api.Public.Configuration.Instances;

public sealed class PasswordCredentialsConfiguration
{
    public required string HashAlgorithmName { get; init; }

    public HashAlgorithm GetHashAlgorithm()
    {
        return new HashAlgorithm(HashAlgorithmName);
    }
}