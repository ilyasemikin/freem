using Freem.Entities.Storage.Abstractions.Base;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface ITagsRepository :
    IWriteRepository<Tag, TagIdentifier>,
    ISearchByIdRepository<Tag, TagIdentifier>
{
    Task<SearchEntityResult<Tag>> FindAsync(TagAndUserIdentifiers ids, CancellationToken cancellationToken = default);
    
    Task<SearchEntitiesAsyncResult<Tag>> FindByUserAsync(
        TagsByUserFilter filter,
        CancellationToken cancellationToken = default);
}
