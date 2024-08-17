namespace Freem.Entities.Storage.Abstractions.Exceptions.Base;

public abstract class StorageBaseException : Exception
{
    protected StorageBaseException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
