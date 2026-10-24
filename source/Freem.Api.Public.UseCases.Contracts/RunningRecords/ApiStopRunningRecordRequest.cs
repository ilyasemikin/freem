using Freem.Api.Public.Contracts.V1.Records.Running;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.RunningRecords;

public sealed class ApiStopRunningRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.RunningRecords.Stop;

    public RunningRecordStopRequest Body { get; }

    public ApiStopRunningRecordRequest(RunningRecordStopRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
