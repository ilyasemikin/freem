using Freem.Entities.Collections.Abstractions;

namespace Freem.Entities.Collections;

public sealed class RelatedCategoriesCollection : RelatedEntitiesCollection<Category>
{
    private const int MinimumRelatedCategoriesCount = 1;

    public RelatedCategoriesCollection(IEnumerable<Category> entities, IEnumerable<string> identifiers) 
        : base(entities, identifiers, MinimumRelatedCategoriesCount)
    {
    }

    public RelatedCategoriesCollection(IEnumerable<Category> entities)
        : this(entities, [])
    {
    }

    public RelatedCategoriesCollection(IEnumerable<string> identifiers)
        : this([], identifiers)
    { 
    }
}
