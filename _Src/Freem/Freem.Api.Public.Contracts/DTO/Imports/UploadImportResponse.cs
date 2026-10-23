namespace Freem.Api.Public.Contracts.DTO.Imports;

/// <summary>Result of uploading a file for import.</summary>
public sealed class UploadImportResponse
{
    /// <summary>The created import.</summary>
    public ImportResponse Import { get; }

    /// <summary>Rows that were skipped or changed while parsing. Not stored with the import.</summary>
    public IReadOnlyList<ImportWarningResponse> Warnings { get; }

    public UploadImportResponse(ImportResponse import, IReadOnlyList<ImportWarningResponse> warnings)
    {
        ArgumentNullException.ThrowIfNull(import);
        ArgumentNullException.ThrowIfNull(warnings);

        Import = import;
        Warnings = warnings;
    }
}
