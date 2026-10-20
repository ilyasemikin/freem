using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Entities.UseCases.Contracts.RunningRecords.Get;

public sealed class GetRunningRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.RunningRecords.Get;

    public static GetRunningRecordRequest Instance { get; } = new();
}