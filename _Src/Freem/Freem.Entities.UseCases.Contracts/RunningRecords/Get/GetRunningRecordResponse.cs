using Freem.Domain.Entities.RunningRecords;

namespace Freem.Entities.UseCases.Contracts.RunningRecords.Get;

public sealed class GetRunningRecordResponse
{
    public RunningRecord Record { get; }

    public GetRunningRecordResponse(RunningRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);

        Record = record;
    }
}