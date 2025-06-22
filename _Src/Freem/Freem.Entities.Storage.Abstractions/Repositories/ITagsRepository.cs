using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Base.Write;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Tags.Models;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface ITagsRepository :
    IWriteRepository<Tag, TagIdentifier>,
    ISearchByIdRepository<Tag, TagIdentifier>,
    ISearchByMultipleIdsRepository<Tag, TagIdentifier, TagAndUserIdentifiers>,
    IMultipleSearchByFilterRepository<Tag, TagIdentifier, TagsByUserFilter>,
    IMultipleSearchByFilterRepository<Tag, TagIdentifier, TagsFilter>
{
}
