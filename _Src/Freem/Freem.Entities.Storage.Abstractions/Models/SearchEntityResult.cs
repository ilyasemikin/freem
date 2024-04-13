using System.Diagnostics.CodeAnalysis;

namespace Freem.Entities.Storage.Abstractions.Models;

public class SearchEntityResult<TEntity>
{
    [MemberNotNullWhen(true, nameof(Entity))]
    public bool Founded { get; }

    public TEntity? Entity { get; }

    private SearchEntityResult(bool founded, TEntity? entity)
    {
        Founded = founded;
        Entity = entity;
    }

    public static SearchEntityResult<TEntity> Found(TEntity entity)
    {
        return new SearchEntityResult<TEntity>(true, entity);
    }

    public static SearchEntityResult<TEntity> NotFound()
    {
        return new SearchEntityResult<TEntity>(false, default);
    }
}
