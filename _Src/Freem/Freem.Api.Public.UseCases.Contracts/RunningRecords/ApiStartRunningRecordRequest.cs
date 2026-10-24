using Freem.Api.Public.Contracts.V1.Records.Running;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.RunningRecords;

public sealed class ApiStartRunningRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.RunningRecords.Start;

    public RunningRecordStartRequest Body { get; }

    public ApiStartRunningRecordRequest(RunningRecordStartRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
