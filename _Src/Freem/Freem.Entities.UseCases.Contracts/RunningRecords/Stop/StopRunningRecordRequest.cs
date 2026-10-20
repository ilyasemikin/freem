using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Entities.UseCases.Contracts.RunningRecords.Stop;

public sealed class StopRunningRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.RunningRecords.Stop;

    public DateTimeOffset EndAt { get; }

    public StopRunningRecordRequest(DateTimeOffset endAt)
    {
        EndAt = endAt;
    }
}