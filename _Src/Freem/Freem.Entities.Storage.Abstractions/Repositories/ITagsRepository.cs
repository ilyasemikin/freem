using Freem.Entities.Storage.Abstractions.Base;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Models.Sorting;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface ITagsRepository :
    IBaseWriteRepository<Tag>,
    IBaseSearchByIdRepository<Tag>,
    IBaseSearchByFilterRepository<Tag, TagFilter, TagSortField>
{
}
