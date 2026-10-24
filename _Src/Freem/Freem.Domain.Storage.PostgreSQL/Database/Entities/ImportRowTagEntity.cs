namespace Freem.Domain.Storage.PostgreSQL.Database.Entities;

internal sealed class ImportRowTagEntity
{
    public required string RowId { get; init; }
    public required string ImportTagId { get; init; }
    public int Position { get; set; }

    public ImportRowEntity? Row { get; set; }
}
