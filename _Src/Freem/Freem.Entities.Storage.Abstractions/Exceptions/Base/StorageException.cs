namespace Freem.Entities.Storage.Abstractions.Exceptions.Base;

public abstract class StorageException : Exception
{
    protected StorageException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
