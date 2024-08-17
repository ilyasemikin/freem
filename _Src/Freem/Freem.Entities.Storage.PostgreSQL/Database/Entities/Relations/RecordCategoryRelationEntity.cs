﻿namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

internal sealed class RecordCategoryRelationEntity
{
    public required string RecordId { get; init; }
    public required string CategoryId { get; init; }

    public RecordEntity? Record { get; set; }
    public CategoryEntity? Category { get; set; }
}
