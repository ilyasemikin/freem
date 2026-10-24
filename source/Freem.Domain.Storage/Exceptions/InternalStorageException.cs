using Freem.Domain.Storage.Exceptions.Base;

namespace Freem.Domain.Storage.Exceptions;

public sealed class InternalStorageException : StorageException
{
    public InternalStorageException(string message, Exception? innerException = null) 
        : base(message, innerException)
    {
    }
}