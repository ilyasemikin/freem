using Freem.Api.Public.Contracts.DTO.Records.Running;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.RunningRecords;

public sealed class ApiStartRunningRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.RunningRecords.Start;

    public StartRunningRecordRequest Body { get; }

    public ApiStartRunningRecordRequest(StartRunningRecordRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
