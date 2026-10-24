using Freem.Domain.Records;

namespace Freem.Api.Public.Contracts.V1.Records;

/// <summary>Result of adding a period to a record.</summary>
public sealed class RecordPeriodAddResponse
{
    /// <summary>Id of the added period.</summary>
    public RecordPeriodId Id { get; }

    public RecordPeriodAddResponse(RecordPeriodId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
