using Freem.Entities.Storage.Abstractions.Base;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Models.Sorting;

namespace Freem.Entities.Storage.Abstractions;

public interface IUsersRepository : 
    IBaseWriteRepository<User>, 
    IBaseSearchByIdRepository<User>,
    IBaseSearchByFilterRepository<User, UserFilter, UserSortField>
{
}
