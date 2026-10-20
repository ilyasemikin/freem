namespace Freem.Api.Public.Contracts.DTO.Records.Running;

/// <summary>Request body for stopping the running record.</summary>
public sealed class StopRunningRecordRequest
{
    /// <summary>When tracking ended. The current time when omitted. Must not be earlier than the start.</summary>
    public DateTimeOffset? EndAt { get; }

    public StopRunningRecordRequest(DateTimeOffset? endAt = null)
    {
        EndAt = endAt;
    }
}