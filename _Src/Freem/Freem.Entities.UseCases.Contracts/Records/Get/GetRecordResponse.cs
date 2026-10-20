using Freem.Domain.Entities.Records;

namespace Freem.Entities.UseCases.Contracts.Records.Get;

public sealed class GetRecordResponse
{
    public Record Record { get; }

    public GetRecordResponse(Record record)
    {
        ArgumentNullException.ThrowIfNull(record);

        Record = record;
    }
}