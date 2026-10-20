using Freem.Domain.Entities.RunningRecords;

namespace Freem.Entities.UseCases.Contracts.RunningRecords.Start;

public sealed class StartRunningRecordResponse
{
    public RunningRecord RunningRecord { get; }

    public StartRunningRecordResponse(RunningRecord runningRecord)
    {
        ArgumentNullException.ThrowIfNull(runningRecord);

        RunningRecord = runningRecord;
    }
}