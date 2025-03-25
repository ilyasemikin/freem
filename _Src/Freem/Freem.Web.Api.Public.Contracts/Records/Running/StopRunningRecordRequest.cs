namespace Freem.Web.Api.Public.Contracts.Records.Running;

public sealed class StopRunningRecordRequest
{
    public DateTimeOffset? EndAt { get; }

    public StopRunningRecordRequest(DateTimeOffset? endAt = null)
    {
        EndAt = endAt;
    }
}