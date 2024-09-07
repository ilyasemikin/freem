using Freem.Converters.Collections;
using Freem.Converters.Collections.Builders;
using Freem.Converters.DependencyInjection.Microsoft.Extensions;
using Freem.Converters.UnitTests.Mocks.Converters;
using Freem.Converters.UnitTests.Mocks.Entities.Abstractions;
using Freem.DependencyInjection.Microsoft;
using Freem.DependencyInjection.Microsoft.Extensions;
using Microsoft.Extensions.DependencyInjection;
using ServiceCollectionExtensions = Freem.Converters.DependencyInjection.Microsoft.Extensions.ServiceCollectionExtensions;

namespace Freem.Converters.UnitTests.Tests.DependencyInjection.Microsoft.Extensions;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddConvertersCollectionWithServiceProvider_ShouldThrowException_WhenServiceCollectionIsNull()
    {
        var exception = Record.Exception(() => 
            ServiceCollectionExtensions
                .AddConvertersCollection<IInputEntity, IOutputEntity>(null!, (_, _) => { }));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("services", ((ArgumentNullException)exception).ParamName);
    }
    
    [Fact]
    public void AddConvertersCollection_ShouldThrowException_WhenServiceCollectionIsNull()
    {
        var exception = Record.Exception(() => 
            ServiceCollectionExtensions
                .AddConvertersCollection<IInputEntity, IOutputEntity>(null!, (_) => { }));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("services", ((ArgumentNullException)exception).ParamName);
    }

    [Fact]
    public void AddConvertersCollectionWithServiceProvider_ShouldThrowException_WhenBuilderActionIsNull()
    {
        var services = new ServiceCollection();

        var exception = Record.Exception(() => services.AddConvertersCollection(
            (Action<IServiceProvider, ConvertersCollectionBuilder<IInputEntity, IOutputEntity>>)null!));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("builderAction", ((ArgumentNullException)exception).ParamName);
    }
    
    [Fact]
    public void AddConvertersCollection_ShouldThrowException_WhenBuilderActionIsNull()
    {
        var services = new ServiceCollection();

        var exception = Record.Exception(() => services.AddConvertersCollection(
            (Action<ConvertersCollectionBuilder<IInputEntity, IOutputEntity>>)null!));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("builderAction", ((ArgumentNullException)exception).ParamName);
    }
    
    [Fact]
    public void AddConvertersCollectionWithServiceProvider_ShouldAddCollection()
    {
        var collection = Services.Resolve<ConvertersCollection<IInputEntity, IOutputEntity>>(services =>
            services.AddConvertersCollection<IInputEntity, IOutputEntity>((provider, builder) =>
            {
                Assert.NotNull(provider);

                builder
                    .Add(new FirstConverter())
                    .Add(new SecondConverter());
            }));

        Assert.NotNull(collection);
        Assert.Equal(2, collection.Count);
    }
    
    [Fact]
    public void AddConvertersCollection_ShouldAddCollection()
    {
        var collection = Services.Resolve<ConvertersCollection<IInputEntity, IOutputEntity>>(services =>
            services.AddConvertersCollection<IInputEntity, IOutputEntity>(builder =>
                builder
                    .Add(new FirstConverter())
                    .Add(new SecondConverter())));

        Assert.NotNull(collection);
        Assert.Equal(2, collection.Count);
    }

    public static TheoryData<ServiceLifetime> ServiceLifetimeCases => new()
    {
        ServiceLifetime.Transient,
        ServiceLifetime.Scoped,
        ServiceLifetime.Singleton
    };
    
    [Theory]
    [MemberData(nameof(ServiceLifetimeCases))]
    public void AddConvertersCollectionWithServiceProvider_ShouldAddWithLifetime(ServiceLifetime lifetime)
    {
        var services = new ServiceCollection();
        services.AddConvertersCollection<IInputEntity, IOutputEntity>((_, _) => { }, lifetime);

        services.TryGetDescriptor<ConvertersCollection<IInputEntity, IOutputEntity>>(out var descriptor);
        
        Assert.NotNull(descriptor);
        Assert.Equal(lifetime, descriptor.Lifetime);
    }
    
    [Theory]
    [MemberData(nameof(ServiceLifetimeCases))]
    public void AddConvertersCollection_ShouldAddWithLifetime(ServiceLifetime lifetime)
    {
        var services = new ServiceCollection();
        services.AddConvertersCollection<IInputEntity, IOutputEntity>(_ => { }, lifetime);

        services.TryGetDescriptor<ConvertersCollection<IInputEntity, IOutputEntity>>(out var descriptor);
        
        Assert.NotNull(descriptor);
        Assert.Equal(lifetime, descriptor.Lifetime);
    }
}