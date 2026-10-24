using Freem.Api.Public.Contracts.V1.Records.Dtos;

namespace Freem.Api.Public.Contracts.V1.Records;

/// <summary>Response containing a record.</summary>
public sealed class RecordResponse
{
    /// <summary>The record.</summary>
    public RecordDto Record { get; }

    public RecordResponse(RecordDto record)
    {
        ArgumentNullException.ThrowIfNull(record);

        Record = record;
    }
}
