using Microsoft.Extensions.DependencyInjection;

namespace Freem.Identifiers.Implementations.Generators.DependencyInjection.Microsoft;

public static class ServiceCollectionExtensions
{
    public static GuidStringIdentifierGeneratorsAdder AddGuidStringIdentifierGenerators(this IServiceCollection services)
    {
        return new GuidStringIdentifierGeneratorsAdder(services);
    }
}