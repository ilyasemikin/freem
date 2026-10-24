using Freem.Api.Public.Contracts.V1.DataTransfer.Dtos;
namespace Freem.Api.Public.Contracts.V1.DataTransfer;

/// <summary>Formats supported for export and import.</summary>
public sealed class FormatListResponse
{
    /// <summary>Formats the user's data can be exported to.</summary>
    public IReadOnlyList<FormatDto> ExportFormats { get; }

    /// <summary>Formats a file can be imported from.</summary>
    public IReadOnlyList<FormatDto> ImportFormats { get; }

    public FormatListResponse(
        IReadOnlyList<FormatDto> exportFormats,
        IReadOnlyList<FormatDto> importFormats)
    {
        ArgumentNullException.ThrowIfNull(exportFormats);
        ArgumentNullException.ThrowIfNull(importFormats);

        ExportFormats = exportFormats;
        ImportFormats = importFormats;
    }
}
