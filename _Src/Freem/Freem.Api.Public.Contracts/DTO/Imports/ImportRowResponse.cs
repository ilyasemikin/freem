using Freem.Domain.Import;
using Freem.Domain.Records.Models;

namespace Freem.Api.Public.Contracts.DTO.Imports;

/// <summary>A row of an import: a record read from the file.</summary>
public sealed class ImportRowResponse
{
    /// <summary>Row id.</summary>
    public ImportRowId Id { get; }

    /// <summary>Key of the record in the imported file.</summary>
    public ImportRecordId RecordId { get; }

    /// <summary>Record name, if set.</summary>
    public RecordName? Name { get; init; }

    /// <summary>Record description, if set.</summary>
    public RecordDescription? Description { get; init; }

    /// <summary>Keys of the import activities of the record.</summary>
    public IReadOnlyList<ImportActivityId> ActivityIds { get; }

    /// <summary>Keys of the import tags of the record.</summary>
    public IReadOnlyList<ImportTagId> TagIds { get; }

    /// <summary>Periods of the record.</summary>
    public IReadOnlyList<ImportRowPeriodResponse> Periods { get; }

    public ImportRowResponse(
        ImportRowId id,
        ImportRecordId recordId,
        IReadOnlyList<ImportActivityId> activityIds,
        IReadOnlyList<ImportTagId> tagIds,
        IReadOnlyList<ImportRowPeriodResponse> periods)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(recordId);
        ArgumentNullException.ThrowIfNull(activityIds);
        ArgumentNullException.ThrowIfNull(tagIds);
        ArgumentNullException.ThrowIfNull(periods);

        Id = id;
        RecordId = recordId;
        ActivityIds = activityIds;
        TagIds = tagIds;
        Periods = periods;
    }
}
