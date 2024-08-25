using Freem.Entities.Identifiers;
using Freem.Entities.Identifiers.Multiple;
using Freem.Entities.Storage.Abstractions.Base;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface ICategoriesRepository :
    IWriteRepository<Category, CategoryIdentifier>,
    ISearchByIdRepository<Category, CategoryIdentifier>
{
    Task<SearchEntityResult<Category>> FindAsync(
        CategoryAndUserIdentifiers ids,
        CancellationToken cancellationToken);

    Task<SearchEntitiesAsyncResult<Category>> FindByUserAsync(
        CategoriesByUserFilter filter, 
        CancellationToken cancellationToken = default);
}
