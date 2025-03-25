namespace Freem.Entities.UseCases.Contracts.RunningRecords.Stop;

public sealed class StopRunningRecordRequest
{
    public DateTimeOffset EndAt { get; }

    public StopRunningRecordRequest(DateTimeOffset endAt)
    {
        EndAt = endAt;
    }
}