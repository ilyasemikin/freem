using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.RunningRecords;

public sealed class ApiRemoveRunningRecordRequest : IRequest
{
    public static readonly ApiRemoveRunningRecordRequest Instance = new();

    public RequestType Type => RequestTypes.RunningRecords.Remove;

    private ApiRemoveRunningRecordRequest()
    {
    }
}
