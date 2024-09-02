using Freem.Entities.Storage.Abstractions.Exceptions.Base;
using Freem.Entities.Storage.PostgreSQL.Database.Errors;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Factories.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Factories.Implementations;

internal class DefaultStorageExceptionFactory : IStorageExceptionFactory
{
    public StorageException Create(Error error)
    {
        throw new NotImplementedException();
    }
}