namespace Freem.Api.Public.Contracts.V1.Records.Running;

/// <summary>Request body for stopping the running record.</summary>
public sealed class RunningRecordStopRequest
{
    /// <summary>When tracking ended. The current time when omitted. Must not be earlier than the start.</summary>
    public DateTimeOffset? EndAt { get; }

    public RunningRecordStopRequest(DateTimeOffset? endAt = null)
    {
        EndAt = endAt;
    }
}