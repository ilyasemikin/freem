using Freem.Api.Public.Contracts.V1.Records.Running.Dtos;

namespace Freem.Api.Public.Contracts.V1.Records.Running;

/// <summary>Response containing the running record, or <see langword="null"/> if no record is running.</summary>
public sealed class RunningRecordResponse
{
    /// <summary>The record, or <see langword="null"/> if no record is running.</summary>
    public RunningRecordDto? RunningRecord { get; }

    public RunningRecordResponse(RunningRecordDto? runningRecord)
    {
        RunningRecord = runningRecord;
    }
}
