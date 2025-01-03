using Freem.Locking.Abstractions;
using Timeout = Freem.Timeouts.Models.Timeout;

namespace Freem.Locking.Local.Implementations.Empty;

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