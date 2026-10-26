namespace Freem.Domain.Records.Sources;

/// <summary>
/// Identifies a <see cref="RecordSource"/> variant without a type check.
/// </summary>
public enum RecordSourceType
{
    /// <summary>Corresponds to <see cref="ManualRecordSource"/>.</summary>
    Manual,

    /// <summary>Corresponds to <see cref="RunningRecordSource"/>.</summary>
    RunningRecord,

    /// <summary>Corresponds to <see cref="ImportRecordSource"/>.</summary>
    Import
}
