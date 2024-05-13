using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;

internal interface ISoftDeletedEntity
{
    DateTimeOffset? DeletedAt { get; set; }
}
