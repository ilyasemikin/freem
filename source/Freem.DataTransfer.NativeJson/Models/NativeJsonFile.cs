namespace Freem.DataTransfer.NativeJson.Models;

public sealed class NativeJsonFile
{
    public string? Format { get; init; }
    public int? Version { get; init; }
    public IReadOnlyList<NativeJsonTag>? Tags { get; init; }
    public IReadOnlyList<NativeJsonActivity>? Activities { get; init; }
    public IReadOnlyList<NativeJsonRecord>? Records { get; init; }
}
