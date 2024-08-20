using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Base;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface ITagsRepository :
    IBaseWriteRepository<Tag, TagIdentifier>,
    IBaseSearchByIdRepository<Tag, TagIdentifier>
{
}
