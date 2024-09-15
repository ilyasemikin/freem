using Freem.Entities.Storage.Abstractions.Exceptions.Base;

namespace Freem.Entities.Storage.Abstractions.Exceptions;

public sealed class InternalStorageException : StorageException
{
    public InternalStorageException(string message, Exception? innerException = null) 
        : base(message, innerException)
    {
    }
}