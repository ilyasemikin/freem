using Freem.DependencyInjection.Microsoft.Extensions;
using Freem.Time.Abstractions;
using Freem.Time.DependencyInjection.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Freem.Time.UnitTests.Tests.DependencyInjection.Microsoft;

public class ServiceCollectionExtensionsTests
{
    [Test]
    public void AddUtcCurrentTimeGetter_ShouldAddGetter()
    {
        var services = new ServiceCollection();

        services.AddUtcCurrentTimeGetter();

        Assert.DoesNotThrow(() => services.BuildAndValidateServiceProvider());
    }

    [Test]
    public void AddUtcCurrentTimeGetter_ShouldAddGetterOnlyOneTime_WhenCallTwice()
    {
        var services = new ServiceCollection();

        services.AddUtcCurrentTimeGetter();
        services.AddUtcCurrentTimeGetter();

        var count = services.CountByServiceType<ICurrentTimeGetter>();
        Assert.That(count, Is.EqualTo(1));
    }
}