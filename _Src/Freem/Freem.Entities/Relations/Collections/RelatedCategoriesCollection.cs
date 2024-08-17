using Freem.Entities.Identifiers;
using Freem.Entities.Relations.Collections.Base;

namespace Freem.Entities.Relations.Collections;

public sealed class RelatedCategoriesCollection : RelatedEntitiesCollection<Category, CategoryIdentifier>
{
    public const int MinCategoriesCount = 1;
    public const int MaxCategoriesCount = 16;

    public RelatedCategoriesCollection(IEnumerable<CategoryIdentifier> identifiers, IEnumerable<Category> entities) 
        : base(identifiers, entities, MinCategoriesCount, MaxCategoriesCount)
    {
    }

    public RelatedCategoriesCollection()
        : this([], [])
    {
    }

    public RelatedCategoriesCollection(IEnumerable<CategoryIdentifier> identifiers)
        : this(identifiers, [])
    { 
    }

    public RelatedCategoriesCollection(IEnumerable<Category> entities)
        : this(Enumerable.Empty<CategoryIdentifier>(), entities)
    {
    }
}
