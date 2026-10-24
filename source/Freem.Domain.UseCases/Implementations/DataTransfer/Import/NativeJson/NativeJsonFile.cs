namespace Freem.Domain.UseCases.Implementations.DataTransfer.Import.NativeJson;

internal sealed class NativeJsonFile
{
    public string? Format { get; init; }
    public int? Version { get; init; }
    public IReadOnlyList<NativeJsonTag>? Tags { get; init; }
    public IReadOnlyList<NativeJsonActivity>? Activities { get; init; }
    public IReadOnlyList<NativeJsonRecord>? Records { get; init; }
}

internal sealed class NativeJsonTag
{
    public string? Id { get; init; }
    public string? Name { get; init; }
}

internal sealed class NativeJsonActivity
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Color { get; init; }
    public string? Status { get; init; }
    public IReadOnlyList<string>? TagIds { get; init; }
}

internal sealed class NativeJsonRecord
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public IReadOnlyList<string>? ActivityIds { get; init; }
    public IReadOnlyList<string>? TagIds { get; init; }
    public IReadOnlyList<NativeJsonPeriod>? Periods { get; init; }
}

internal sealed class NativeJsonPeriod
{
    public string? Id { get; init; }
    public DateTimeOffset? StartAt { get; init; }
    public DateTimeOffset? EndAt { get; init; }
    public string? TimeZoneOffset { get; init; }
    public string? Description { get; init; }
}
