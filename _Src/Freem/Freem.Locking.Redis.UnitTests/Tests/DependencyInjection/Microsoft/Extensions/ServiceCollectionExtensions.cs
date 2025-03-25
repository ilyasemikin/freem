using Freem.DependencyInjection.Microsoft.Extensions;
using Freem.Locking.Redis.DependencyInjection.Microsoft;
using Freem.Locking.Redis.DependencyInjection.Microsoft.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Locking.Redis.UnitTests.Tests.DependencyInjection.Microsoft.Extensions;

public sealed class ServiceCollectionExtensions
{
    [Fact]
    public void AddSimpleRedisDistributedLocks_ShouldBuildValidProvider()
    {
        var services = new ServiceCollection();

        var sampleConfiguration = Guid.NewGuid().ToString();
        var configuration = new RedisConfiguration(sampleConfiguration);
        
        services.AddSimpleRedisDistributedLocks(configuration);

        IServiceProvider? provider = null;
        var exception = Record.Exception(() => provider = services.BuildAndValidateServiceProvider());
        
        Assert.Null(exception);
        Assert.NotNull(provider);
    }
}