using Freem.Entities.Collections.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freem.Entities.Collections;

public class RelatedTagsCollection : RelatedEntitiesCollection<Tag>
{
    public RelatedTagsCollection(IEnumerable<Tag> entities, IEnumerable<string> identifiers) 
        : base(entities, identifiers)
    {
    }

    public RelatedTagsCollection(IEnumerable<Tag> entities)
        : this(entities, [])
    {
    }

    public RelatedTagsCollection(IEnumerable<string> identifiers)
        : this([], identifiers)
    {
    }
}
