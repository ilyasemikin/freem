namespace Freem.Api.Public.Contracts.DTO.DataTransfer;

/// <summary>Formats supported for export and import.</summary>
public sealed class ListFormatsResponse
{
    /// <summary>Formats the user's data can be exported to.</summary>
    public IReadOnlyList<FormatResponse> ExportFormats { get; }

    /// <summary>Formats a file can be imported from.</summary>
    public IReadOnlyList<FormatResponse> ImportFormats { get; }

    public ListFormatsResponse(
        IReadOnlyList<FormatResponse> exportFormats,
        IReadOnlyList<FormatResponse> importFormats)
    {
        ArgumentNullException.ThrowIfNull(exportFormats);
        ArgumentNullException.ThrowIfNull(importFormats);

        ExportFormats = exportFormats;
        ImportFormats = importFormats;
    }
}
