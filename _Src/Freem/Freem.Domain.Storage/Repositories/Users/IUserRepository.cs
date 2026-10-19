using Freem.Domain.Entities.Users;
using Freem.Domain.Storage.Models;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Storage.Repositories.Users;

public interface IUserRepository
{
    Task CreateAsync(User entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(User entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(UserIdentifier id, CancellationToken cancellationToken = default);

    Task<SearchEntityResult<User>> FindByIdAsync(UserIdentifier id, CancellationToken cancellationToken = default);
}