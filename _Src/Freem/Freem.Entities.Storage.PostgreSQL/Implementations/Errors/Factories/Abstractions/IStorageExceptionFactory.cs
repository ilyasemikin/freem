using Freem.Entities.Storage.Abstractions.Exceptions.Base;
using Freem.Entities.Storage.PostgreSQL.Database.Errors;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Factories.Abstractions;

internal interface IStorageExceptionFactory
{
    StorageException Create(Error error);
}