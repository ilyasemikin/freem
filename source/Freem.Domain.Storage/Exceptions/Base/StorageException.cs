namespace Freem.Domain.Storage.Exceptions.Base;

public abstract class StorageException : Exception
{
    protected StorageException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
