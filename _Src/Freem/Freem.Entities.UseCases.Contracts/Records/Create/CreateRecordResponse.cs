using Freem.Domain.Entities.Records;

namespace Freem.Entities.UseCases.Contracts.Records.Create;

public sealed class CreateRecordResponse
{
    public Record Record { get; }

    public CreateRecordResponse(Record record)
    {
        ArgumentNullException.ThrowIfNull(record);

        Record = record;
    }
}