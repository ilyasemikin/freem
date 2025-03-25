namespace Freem.Locking.Abstractions.Exceptions;

public class CantReleaseException : Exception
{
    public string Key { get; }

    public CantReleaseException(string key)
        : base($"Can't release \"{key}\"")
    {
        Key = key;
    }
}