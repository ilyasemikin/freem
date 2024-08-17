using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Base;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IUsersRepository :
    IBaseWriteRepository<User, UserIdentifier>,
    IBaseSearchByIdRepository<User, UserIdentifier>
{
}
