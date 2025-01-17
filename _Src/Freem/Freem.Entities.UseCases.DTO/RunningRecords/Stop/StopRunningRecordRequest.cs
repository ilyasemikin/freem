namespace Freem.Entities.UseCases.DTO.RunningRecords.Stop;

public sealed class StopRunningRecordRequest
{
    public DateTimeOffset EndAt { get; }

    public StopRunningRecordRequest(DateTimeOffset endAt)
    {
        EndAt = endAt;
    }
}