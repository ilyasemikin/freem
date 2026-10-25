namespace Freem.DataTransfer.NativeJson.Models;

public sealed class NativeJsonRecord : NativeJsonItem
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public IReadOnlyList<string>? ActivityIds { get; init; }
    public IReadOnlyList<string>? TagIds { get; init; }
    public IReadOnlyList<NativeJsonPeriod>? Periods { get; init; }
}
