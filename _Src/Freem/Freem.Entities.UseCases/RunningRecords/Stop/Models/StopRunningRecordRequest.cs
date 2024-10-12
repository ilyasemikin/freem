namespace Freem.Entities.UseCases.RunningRecords.Stop.Models;

public sealed class StopRunningRecordRequest
{
    public DateTimeOffset EndAt { get; }

    public StopRunningRecordRequest(DateTimeOffset endAt)
    {
        EndAt = endAt;
    }
}