using Freem.Api.Public.Contracts.V1.Records.Running.Dtos;

namespace Freem.Api.Public.Contracts.V1.Records.Running;

/// <summary>Response containing the running record.</summary>
public sealed class RunningRecordResponse
{
    /// <summary>The record.</summary>
    public RunningRecordDto RunningRecord { get; }

    public RunningRecordResponse(RunningRecordDto runningRecord)
    {
        ArgumentNullException.ThrowIfNull(runningRecord);

        RunningRecord = runningRecord;
    }
}
