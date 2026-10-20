using Freem.Web.Api.Public.Contracts.DTO.Records.Running;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.RunningRecords;

public sealed class ApiUpdateRunningRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.RunningRecords.Update;

    public UpdateRunningRecordRequest Body { get; }

    public ApiUpdateRunningRecordRequest(UpdateRunningRecordRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
