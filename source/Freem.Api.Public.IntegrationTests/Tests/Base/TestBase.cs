using Freem.Api.Public.IntegrationTests.Context;

namespace Freem.Api.Public.IntegrationTests.Tests.Base;

// Every test signs up its own EgoHub user, so data of parallel tests never overlaps.
public abstract class TestBase : IAsyncLifetime
{
    protected TestContext Context { get; } = new();

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        Context.Dispose();

        return Task.CompletedTask;
    }
}
