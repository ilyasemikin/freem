using Freem.Entities.Storage.Abstractions.Models;

namespace Freem.Entities.Storage.Abstractions.Base;

public interface IBaseSearchByIdRepository<TEntity>
{
    Task<SearchEntityResult<TEntity>> FindByIdAsync(string id, CancellationToken cancellationToken = default);
}
