using Freem.Entities.RunningRecords;

namespace Freem.Entities.UseCases.RunningRecords.Start.Models;

public sealed class StartRunningRecordResponse
{
    public RunningRecord RunningRecord { get; }

    public StartRunningRecordResponse(RunningRecord entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        RunningRecord = entity;
    }
}