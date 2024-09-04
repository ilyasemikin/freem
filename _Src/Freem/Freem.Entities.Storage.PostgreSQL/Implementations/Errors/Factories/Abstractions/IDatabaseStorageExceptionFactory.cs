using Freem.Entities.Storage.Abstractions.Exceptions.Base;
using Freem.Entities.Storage.PostgreSQL.Database.Errors;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Factories.Abstractions;

internal interface IDatabaseStorageExceptionFactory
{
    Exception Create(IDatabaseError error);
}