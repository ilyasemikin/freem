using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal sealed class UserEntity : IRowVersionableEntity, IAuditableEntity, ISoftDeletedEntity
{
    public required string Id { get; init; }

    public required string Nickname { get; set; }

    [Timestamp]
    public uint RowVersion { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }
}
