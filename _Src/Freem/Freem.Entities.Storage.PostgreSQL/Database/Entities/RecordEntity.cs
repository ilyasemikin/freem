﻿using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal sealed class RecordEntity : IRowVersionableEntity
{
    public required string Id { get; init; }
    public required string UserId { get; init; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset EndAt { get; set; }

    public UserEntity? User { get; set; }
    public ICollection<CategoryEntity>? Categories { get; set; }
    public ICollection<TagEntity>? Tags { get; set; }

    [Timestamp]
    public uint RowVersion { get; private set; }
}
