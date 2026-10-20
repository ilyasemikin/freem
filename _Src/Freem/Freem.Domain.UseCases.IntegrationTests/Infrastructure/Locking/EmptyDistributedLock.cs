using Freem.Locking;
using Timeout = Freem.Locking.Timeouts.Models.Timeout;

namespace Freem.Domain.UseCases.IntegrationTests.Infrastructure.Locking;

internal sealed class EmptyDistributedLock : IDistributedLock
{
    public static EmptyDistributedLock Instance { get; } = new();
    
    public DateTimeOffset? Expires { get; }

    private EmptyDistributedLock()
    {
        Expires = null;
    }
    
    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
    
    public Task ReleaseAsync(Timeout? timeout, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}