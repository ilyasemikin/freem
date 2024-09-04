using Freem.Entities.Storage.Abstractions.Exceptions.Base;
using Freem.Entities.Storage.PostgreSQL.Database.Errors;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Abstractions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Factories.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Factories.Implementations;

internal class DefaultDatabaseStorageExceptionFactory : IDatabaseStorageExceptionFactory
{
    public Exception Create(IDatabaseError error)
    {
        throw new NotImplementedException();
    }
}