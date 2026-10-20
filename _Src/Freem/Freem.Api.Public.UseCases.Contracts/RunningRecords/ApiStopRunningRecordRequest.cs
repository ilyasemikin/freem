using Freem.Api.Public.Contracts.DTO.Records.Running;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.RunningRecords;

public sealed class ApiStopRunningRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.RunningRecords.Stop;

    public StopRunningRecordRequest Body { get; }

    public ApiStopRunningRecordRequest(StopRunningRecordRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
