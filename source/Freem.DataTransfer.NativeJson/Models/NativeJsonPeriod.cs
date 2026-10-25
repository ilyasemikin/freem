namespace Freem.DataTransfer.NativeJson.Models;

public sealed class NativeJsonPeriod
{
    public string? Id { get; init; }
    public DateTimeOffset? StartAt { get; init; }
    public DateTimeOffset? EndAt { get; init; }
    public string? TimeZoneOffset { get; init; }
    public string? Description { get; init; }
}
