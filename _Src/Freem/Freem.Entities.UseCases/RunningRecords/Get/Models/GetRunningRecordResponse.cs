using System.Diagnostics.CodeAnalysis;
using Freem.Entities.RunningRecords;

namespace Freem.Entities.UseCases.RunningRecords.Get.Models;

public sealed class GetRunningRecordResponse
{
    [MemberNotNullWhen(true, nameof(Record))]
    public bool Founded { get; }
    public RunningRecord? Record { get; }

    public GetRunningRecordResponse(RunningRecord? record)
    {
        Founded = record is not null;
        Record = record;
    }
}