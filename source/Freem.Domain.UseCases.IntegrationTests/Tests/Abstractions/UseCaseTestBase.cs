using Freem.Domain.UseCases.IntegrationTests.Fixtures;

namespace Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;

[Collection("Sequential")]
public abstract class UseCaseTestBase : IAsyncLifetime, IDisposable
{
    protected TestContext Context { get; }

    protected UseCaseTestBase(TestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        Context = context;
        
        Context.CleanDatabases();
    }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Context.CleanDatabases();
        
        GC.SuppressFinalize(this);
    }
}