using Freem.Entities.Identifiers;
using Freem.Entities.Models.Users;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Base.Write;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Users;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IUsersRepository :
    IWriteRepository<User, UserIdentifier>,
    ISearchByIdRepository<User, UserIdentifier>
{
    Task<SearchEntityResult<User>> FindByLoginAsync(Login login, CancellationToken cancellationToken = default);
}
