using Freem.Identifiers.Base;
using StackExchange.Redis;

namespace Freem.Locking.Redis.Implementations.Simple;

internal sealed class SimpleLockIdentifier : StringIdentifier
{
    public SimpleLockIdentifier(string value) 
        : base(value)
    {
    }

    public static implicit operator RedisValue(SimpleLockIdentifier identifier)
    {
        return new RedisValue(identifier);
    }
}