using Freem.Api.Public.IntegrationTests.Context;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.Base;

[Collection("Tests")]
public abstract class TestBase : IAsyncLifetime, IDisposable
{
    protected TestContext Context { get; }

    protected TestBase(TestContext context, ITestOutputHelper? output = null)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        Context = context;
        Context.Output = output;
        
        Context.CleanDatabase();
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
        Context.ClientSession.Clean();
        Context.CleanDatabase();
        
        GC.SuppressFinalize(this);
    }
}
