using Freem.Domain.Entities.Tags;
using Freem.Domain.Storage.Models;
using Freem.Domain.Storage.Models.Identifiers;
using Freem.Domain.Tags.Identifiers;

namespace Freem.Domain.Storage.Repositories.Tags;

public interface ITagRepository
{
    Task CreateAsync(Tag entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Tag entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TagIdentifier id, CancellationToken cancellationToken = default);

    Task<SearchEntityResult<Tag>> FindByIdAsync(TagIdentifier id, CancellationToken cancellationToken = default);
    Task<SearchEntityResult<Tag>> FindByMultipleIdAsync(
        TagAndUserIdentifiers ids,
        CancellationToken cancellationToken = default);

    Task<SearchEntitiesAsyncResult<Tag>> FindAsync(
        TagByUserFilter filter,
        CancellationToken cancellationToken = default);
    Task<SearchEntitiesAsyncResult<Tag>> FindAsync(TagFilter filter, CancellationToken cancellationToken = default);
}