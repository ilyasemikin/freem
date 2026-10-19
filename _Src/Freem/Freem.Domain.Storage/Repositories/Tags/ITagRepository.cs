using Freem.Domain.Entities.Tags;
using Freem.Domain.Storage.Models;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Tags;

namespace Freem.Domain.Storage.Repositories.Tags;

public interface ITagRepository
{
    Task CreateAsync(Tag entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Tag entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TagId id, CancellationToken cancellationToken = default);

    Task<SearchEntityResult<Tag>> FindByIdAsync(TagId id, CancellationToken cancellationToken = default);
    Task<SearchEntityResult<Tag>> FindByMultipleIdAsync(
        TagAndUserIds ids,
        CancellationToken cancellationToken = default);

    Task<SearchEntitiesAsyncResult<Tag>> FindAsync(
        TagByUserFilter filter,
        CancellationToken cancellationToken = default);
    Task<SearchEntitiesAsyncResult<Tag>> FindAsync(TagFilter filter, CancellationToken cancellationToken = default);
}