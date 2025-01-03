using Freem.Entities.UseCases.IntegrationTests.Fixtures;

namespace Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

[Collection("Sequential")]
public abstract class UseCaseTestBase : IClassFixture<ServicesContext>, IDisposable
{
    protected ServicesContext Context { get; }

    public UseCaseTestBase(ServicesContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        Context = context;
        
        Context.DataManager.Clean();
    }

    public void Dispose()
    {
        Context.DataManager.Clean();
        
        GC.SuppressFinalize(this);
    }
}