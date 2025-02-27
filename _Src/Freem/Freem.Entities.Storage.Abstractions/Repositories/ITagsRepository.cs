using Freem.Entities.Identifiers;
using Freem.Entities.Models.Tags;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Base.Write;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Tags;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface ITagsRepository :
    IWriteRepository<Tag, TagIdentifier>,
    ISearchByIdRepository<Tag, TagIdentifier>,
    ISearchByMultipleIdsRepository<Tag, TagIdentifier, TagAndUserIdentifiers>,
    IMultipleSearchByFilterRepository<Tag, TagIdentifier, TagsByUserFilter>
{
    Task<SearchEntityResult<Tag>> FindByNameAsync(TagName name, CancellationToken cancellationToken = default);
}
