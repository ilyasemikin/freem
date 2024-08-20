using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Base;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface ICategoriesRepository :
    IBaseWriteRepository<Category, CategoryIdentifier>,
    IBaseSearchByIdRepository<Category, CategoryIdentifier>
{
}
