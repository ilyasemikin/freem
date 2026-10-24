using Freem.Api.Public.Contracts.V1.Imports.Dtos;

namespace Freem.Api.Public.Contracts.V1.Imports;

/// <summary>Response containing an import.</summary>
public sealed class ImportResponse
{
    /// <summary>The import.</summary>
    public ImportDto Import { get; }

    public ImportResponse(ImportDto import)
    {
        ArgumentNullException.ThrowIfNull(import);

        Import = import;
    }
}
