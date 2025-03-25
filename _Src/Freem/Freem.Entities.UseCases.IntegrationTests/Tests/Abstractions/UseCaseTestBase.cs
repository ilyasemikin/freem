using Freem.Entities.UseCases.IntegrationTests.Fixtures;

namespace Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

[Collection("Sequential")]
public abstract class UseCaseTestBase : IDisposable
{
    protected TestContext Context { get; }

    protected UseCaseTestBase(TestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        Context = context;
        
        Context.CleanDatabases();
    }

    public void Dispose()
    {
        Context.CleanDatabases();
        
        GC.SuppressFinalize(this);
    }
}