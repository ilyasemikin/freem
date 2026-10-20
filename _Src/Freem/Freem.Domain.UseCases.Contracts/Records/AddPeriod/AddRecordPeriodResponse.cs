using Freem.Domain.Records;

namespace Freem.Domain.UseCases.Contracts.Records.AddPeriod;

public sealed class AddRecordPeriodResponse
{
    public RecordPeriodId Id { get; }

    public AddRecordPeriodResponse(RecordPeriodId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
