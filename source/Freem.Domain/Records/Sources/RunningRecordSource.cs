namespace Freem.Domain.Records.Sources;

/// <summary>
/// The record was created by stopping the running record.
/// </summary>
public sealed class RunningRecordSource : RecordSource
{
    public static RunningRecordSource Instance { get; } = new();

    public override RecordSourceType Type => RecordSourceType.RunningRecord;

    private RunningRecordSource()
    {
    }

    public override bool Equals(RecordSource? other)
    {
        return other is RunningRecordSource;
    }

    public override int GetHashCode()
    {
        return (int)Type;
    }
}
