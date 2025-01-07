using Freem.Entities.UseCases.IntegrationTests.Fixtures;

namespace Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

[Collection("Sequential")]
public abstract class UseCaseTestBase
{
    protected ServicesContext Services { get; }

    protected UseCaseTestBase(ServicesContext services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        Services = services;
        
        Services.DataManager.Clean();
    }

    public void Dispose()
    {
        Services.DataManager.Clean();
        
        GC.SuppressFinalize(this);
    }
}