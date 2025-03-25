namespace Freem.Locking.Abstractions.Exceptions;

public sealed class CantLockException : Exception
{
    public string Key { get; }

    public CantLockException(string key)
        : base($"Can't lock \"{key}\"")
    {
        Key = key;
    }
}