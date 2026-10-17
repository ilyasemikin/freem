using Freem.DependencyInjection.Microsoft.Extensions;
using Freem.Time.Abstractions;
using Freem.Time.DependencyInjection.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Freem.Time.UnitTests.Tests.DependencyInjection.Microsoft;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddUtcCurrentTimeGetter_ShouldAddGetter()
    {
        var services = new ServiceCollection();

        services.AddUtcCurrentTimeGetter();

        services.BuildAndValidateServiceProvider();
    }

    [Fact]
    public void AddUtcCurrentTimeGetter_ShouldAddGetterOnlyOneTime_WhenCallTwice()
    {
        var services = new ServiceCollection();

        services.AddUtcCurrentTimeGetter();
        services.AddUtcCurrentTimeGetter();

        var count = services.Count(service => service.ServiceType == typeof(ICurrentTimeGetter));
        Assert.Equal(1, count);
    }
}