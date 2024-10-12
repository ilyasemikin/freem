using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Records;

namespace Freem.Entities.UseCases.Records.Get.Models;

public sealed class GetRecordResponse
{
    [MemberNotNullWhen(true, nameof(Record))]
    public bool Founded { get; }
    public Record? Record { get; }
    
    public GetRecordResponse(Record? record)
    {
        Founded = record is not null;
        Record = record;
    }
}