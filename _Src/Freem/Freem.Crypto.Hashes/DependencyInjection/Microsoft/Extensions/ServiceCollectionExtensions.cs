using Freem.Crypto.Hashes.Abstractions;
using Freem.Crypto.Hashes.Implementations;
using Freem.Crypto.Hashes.Implementations.Hashers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Crypto.Hashes.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCryptoHashes(this IServiceCollection services)
    {
        var hashers = new RawHasher[] { new Sha512RawHasher() };
        var collection = new RawHasherCollection(hashers);
        
        services.TryAddSingleton(collection);
        
        return services;
    }
}