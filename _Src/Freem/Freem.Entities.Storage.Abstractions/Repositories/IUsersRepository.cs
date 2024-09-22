using Freem.Entities.Storage.Abstractions.Base;
using Freem.Entities.Users;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IUsersRepository :
    IWriteRepository<User, UserIdentifier>,
    ISearchByIdRepository<User, UserIdentifier>
{
}
