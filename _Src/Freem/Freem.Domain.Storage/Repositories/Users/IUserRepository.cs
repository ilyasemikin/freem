using Freem.Domain.Entities.Users;
using Freem.Domain.Storage.Models;
using Freem.Domain.Users;

namespace Freem.Domain.Storage.Repositories.Users;

public interface IUserRepository
{
    Task CreateAsync(User entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(User entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(UserId id, CancellationToken cancellationToken = default);

    Task<SearchEntityResult<User>> FindByIdAsync(UserId id, CancellationToken cancellationToken = default);
}