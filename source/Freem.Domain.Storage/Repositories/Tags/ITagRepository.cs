using Freem.Domain.Entities.Tags;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Tags;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.Repositories.Tags;

public interface ITagRepository
{
    Task<Result<Error>> CreateAsync(Tag entity, CancellationToken cancellationToken = default);
    Task<Result<Error>> UpdateAsync(Tag entity, CancellationToken cancellationToken = default);
    Task<Result<Error>> DeleteAsync(TagId id, CancellationToken cancellationToken = default);

    Task<Result<Tag, Error>> FindByIdAsync(TagId id, CancellationToken cancellationToken = default);
    Task<Result<Tag, Error>> FindByMultipleIdAsync(
        TagAndUserIds ids,
        CancellationToken cancellationToken = default);

    Task<Result<AsyncBatch<Tag>, Error>> FindAsync(
        TagByUserFilter filter,
        CancellationToken cancellationToken = default);
    Task<Result<AsyncBatch<Tag>, Error>> FindAsync(
        TagFilter filter,
        CancellationToken cancellationToken = default);
}