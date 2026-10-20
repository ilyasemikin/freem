using Freem.Domain.Records;

namespace Freem.Api.Public.Contracts.DTO.Records;

/// <summary>Result of adding a period to a record.</summary>
public sealed class AddRecordPeriodResponse
{
    /// <summary>Id of the added period.</summary>
    public RecordPeriodId Id { get; }

    public AddRecordPeriodResponse(RecordPeriodId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
