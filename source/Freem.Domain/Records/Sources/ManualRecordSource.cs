namespace Freem.Domain.Records.Sources;

/// <summary>
/// The record was created directly by the user.
/// </summary>
public sealed class ManualRecordSource : RecordSource
{
    public static ManualRecordSource Instance { get; } = new();

    public override RecordSourceType Type => RecordSourceType.Manual;

    private ManualRecordSource()
    {
    }

    public override bool Equals(RecordSource? other)
    {
        return other is ManualRecordSource;
    }

    public override int GetHashCode()
    {
        return (int)Type;
    }
}
