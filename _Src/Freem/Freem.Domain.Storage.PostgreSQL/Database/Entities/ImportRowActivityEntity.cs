namespace Freem.Domain.Storage.PostgreSQL.Database.Entities;

internal sealed class ImportRowActivityEntity
{
    public required string RowId { get; init; }
    public required string SourceActivityId { get; init; }
    public int Position { get; set; }

    public ImportRowEntity? Row { get; set; }
}
