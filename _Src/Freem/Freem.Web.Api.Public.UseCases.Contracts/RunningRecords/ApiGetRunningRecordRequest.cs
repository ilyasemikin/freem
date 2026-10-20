using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.RunningRecords;

public sealed class ApiGetRunningRecordRequest : IRequest
{
    public static readonly ApiGetRunningRecordRequest Instance = new();

    public RequestType Type => RequestTypes.RunningRecords.Get;

    private ApiGetRunningRecordRequest()
    {
    }
}
