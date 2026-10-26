using Freem.Domain.DataTransfer.Formats;
using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Records.Sources;

namespace Freem.Api.Public.Contracts.V1.Records.Dtos;

/// <summary>How a record came to exist.</summary>
public sealed class RecordSourceDto
{
    /// <summary>Source variant: <c>manual</c>, <c>running_record</c> or <c>import</c>.</summary>
    public RecordSourceType Type { get; }

    /// <summary>Import the record was created from; set only for <c>import</c>, may refer to a removed import.</summary>
    public ImportDocumentId? ImportId { get; init; }

    /// <summary>Import row the record was created from; set only for <c>import</c>, may refer to a removed import.</summary>
    public ImportRowId? ImportRowId { get; init; }

    /// <summary>Key of the import format; set only for <c>import</c>.</summary>
    public FormatKey? Format { get; init; }

    /// <summary>Record key in the imported file; set only for <c>import</c>.</summary>
    public ImportRecordId? ImportRecordId { get; init; }

    public RecordSourceDto(RecordSourceType type)
    {
        Type = type;
    }
}
