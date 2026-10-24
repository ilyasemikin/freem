using Freem.Domain.Storage.Repositories.Activities;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Storage.Repositories.Tags;
using Freem.Domain.UseCases.DependencyInjection;
using Freem.Domain.UseCases.Implementations.Export;
using Freem.Domain.UseCases.Implementations.Import;
using Freem.Domain.UseCases.UnitTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Domain.UseCases.UnitTests.Formats;

public sealed class FormatRegistrationTests
{
    private static readonly Type[] AssemblyTypes = typeof(IExportWriter).Assembly.GetTypes();

    [Fact]
    public void AllExportWriters_ShouldBeRegistered()
    {
        var implementations = FindImplementations<IExportWriter>();
        var services = CreateServices();

        var registered = services
            .Where(descriptor => descriptor.ServiceType == typeof(IExportWriter))
            .Select(descriptor => descriptor.ImplementationType)
            .ToArray();

        Assert.NotEmpty(implementations);
        Assert.Equal(implementations.OrderBy(type => type.FullName), registered.OrderBy(type => type!.FullName));
    }

    [Fact]
    public void AllImportParsers_ShouldBeRegistered()
    {
        var implementations = FindImplementations<IImportParser>();
        var services = CreateServices();

        var registered = services
            .Where(descriptor => descriptor.ServiceType == typeof(IImportParser))
            .Select(descriptor => descriptor.ImplementationType)
            .ToArray();

        Assert.NotEmpty(implementations);
        Assert.Equal(implementations.OrderBy(type => type.FullName), registered.OrderBy(type => type!.FullName));
    }

    [Fact]
    public void Registries_ShouldBeResolvedWithUniqueKeys()
    {
        var services = CreateServices();
        using var provider = services.BuildServiceProvider();

        var exportRegistry = provider.GetRequiredService<ExportWriterRegistry>();
        var importRegistry = provider.GetRequiredService<ImportParserRegistry>();

        Assert.NotEmpty(exportRegistry.Formats);
        Assert.NotEmpty(importRegistry.Formats);
    }

    [Fact]
    public void ExportItemReader_ShouldBeResolved()
    {
        var services = CreateServices();
        services.AddSingleton<ITagRepository>(new FakeTagRepository([]));
        services.AddSingleton<IActivityRepository>(new FakeActivityRepository([]));
        services.AddSingleton<IRecordRepository>(new FakeRecordRepository([]));
        using var provider = services.BuildServiceProvider();

        var reader = provider.GetRequiredService<ExportItemReader>();

        Assert.NotNull(reader);
    }

    private static Type[] FindImplementations<T>()
    {
        return AssemblyTypes
            .Where(type => type is { IsClass: true, IsAbstract: false } && typeof(T).IsAssignableFrom(type))
            .ToArray();
    }

    private static IServiceCollection CreateServices()
    {
        var services = new ServiceCollection();
        services.AddEntitiesUseCases();
        return services;
    }
}
