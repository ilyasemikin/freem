using Freem.Domain.Records.Models;
using Freem.Time;

namespace Freem.Api.Public.Contracts.V1.Records;

/// <summary>Request body for updating a period of a record. Fields that are omitted stay unchanged.</summary>
public sealed class RecordPeriodUpdateRequest
{
    /// <summary>New start and end of the period. The duration must be positive.</summary>
    public UpdateField<DateTimePeriod>? Period { get; init; }

    /// <summary>New period description; a null value clears it.</summary>
    public UpdateField<RecordPeriodDescription?>? Description { get; init; }

    /// <summary>Whether at least one field is set.</summary>
    public bool HasChanges()
    {
        return
            Period is not null ||
            Description is not null;
    }
}
