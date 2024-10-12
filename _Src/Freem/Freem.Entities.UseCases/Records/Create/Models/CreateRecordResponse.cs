using Freem.Entities.Records;

namespace Freem.Entities.UseCases.Records.Create.Models;

public sealed class CreateRecordResponse
{
    public Record Record { get; }

    public CreateRecordResponse(Record record)
    {
        ArgumentNullException.ThrowIfNull(record);
        
        Record = record;
    }
}