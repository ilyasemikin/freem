using Freem.Entities.Storage.Abstractions.Exceptions.Base;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Exceptions;

internal class InternalStorageException : StorageException
{
    public InternalStorageException(string message) 
        : base(message)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);
    }
}