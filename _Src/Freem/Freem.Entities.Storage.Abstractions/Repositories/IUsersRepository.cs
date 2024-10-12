using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Base.Write;
using Freem.Entities.Users;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IUsersRepository :
    IWriteRepository<User, UserIdentifier>,
    ISearchByIdRepository<User, UserIdentifier>
{
}
