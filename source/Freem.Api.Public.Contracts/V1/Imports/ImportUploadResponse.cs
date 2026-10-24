using Freem.Api.Public.Contracts.V1.Imports.Dtos;
namespace Freem.Api.Public.Contracts.V1.Imports;

/// <summary>Result of uploading a file for import.</summary>
public sealed class ImportUploadResponse
{
    /// <summary>The created import.</summary>
    public ImportDto Import { get; }

    /// <summary>Rows that were skipped or changed while parsing. Not stored with the import.</summary>
    public IReadOnlyList<ImportWarningDto> Warnings { get; }

    public ImportUploadResponse(ImportDto import, IReadOnlyList<ImportWarningDto> warnings)
    {
        ArgumentNullException.ThrowIfNull(import);
        ArgumentNullException.ThrowIfNull(warnings);

        Import = import;
        Warnings = warnings;
    }
}
