using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

internal sealed class RunningRecordCategoryRelationEntity
{
    public required string RunningRecordId { get; init; }
    public required string CategoryId { get; init; }

    public RunningRecordEntity? RunningRecord { get; set; }
    public CategoryEntity? Category { get; set; }
}
