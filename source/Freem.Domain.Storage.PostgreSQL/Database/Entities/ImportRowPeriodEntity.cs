namespace Freem.Domain.Storage.PostgreSQL.Database.Entities;

internal sealed class ImportRowPeriodEntity
{
    public required string RowId { get; init; }
    public int Position { get; init; }
    public string? Description { get; set; }
    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset EndAt { get; set; }
    public long TimeZoneOffsetTicks { get; set; }

    public ImportRowEntity? Row { get; set; }
}
