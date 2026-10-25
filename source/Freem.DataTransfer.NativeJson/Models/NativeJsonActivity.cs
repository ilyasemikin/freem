namespace Freem.DataTransfer.NativeJson.Models;

public sealed class NativeJsonActivity : NativeJsonItem
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Color { get; init; }
    public string? Status { get; init; }
    public IReadOnlyList<string>? TagIds { get; init; }
}
