using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.RunningRecords.Remove;

public sealed class RemoveRunningRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.RunningRecords.Remove;

    public static RemoveRunningRecordRequest Instance { get; } = new();
}