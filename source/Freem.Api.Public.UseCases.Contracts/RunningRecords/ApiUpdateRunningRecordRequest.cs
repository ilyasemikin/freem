using Freem.Api.Public.Contracts.V1.Records.Running;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.RunningRecords;

public sealed class ApiUpdateRunningRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.RunningRecords.Update;

    public RunningRecordUpdateRequest Body { get; }

    public ApiUpdateRunningRecordRequest(RunningRecordUpdateRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
