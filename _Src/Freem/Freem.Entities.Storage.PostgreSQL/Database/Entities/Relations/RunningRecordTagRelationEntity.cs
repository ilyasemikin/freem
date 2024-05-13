using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

internal sealed class RunningRecordTagRelationEntity
{
    public required string RunningRecordId { get; init; }
    public required string TagId { get; init; }

    public RunningRecordEntity? RunningRecord { get; set; }
    public TagEntity? Tag { get; set; }
}
