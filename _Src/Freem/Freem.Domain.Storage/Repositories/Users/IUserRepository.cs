using Freem.Domain.Entities.Users;
using Freem.Domain.Users;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.Repositories.Users;

public interface IUserRepository
{
    Task<Result<Error>> CreateAsync(User entity, CancellationToken cancellationToken = default);
    Task<Result<Error>> UpdateAsync(User entity, CancellationToken cancellationToken = default);
    Task<Result<Error>> DeleteAsync(UserId id, CancellationToken cancellationToken = default);

    Task<Result<User, Error>> FindByIdAsync(UserId id, CancellationToken cancellationToken = default);
}