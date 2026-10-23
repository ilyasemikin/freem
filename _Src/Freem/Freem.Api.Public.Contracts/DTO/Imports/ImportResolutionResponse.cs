namespace Freem.Api.Public.Contracts.DTO.Imports;

/// <summary>Decision on what a source activity or tag maps to in Freem.</summary>
/// <typeparam name="TId">Id of the Freem entity.</typeparam>
public sealed class ImportResolutionResponse<TId>
    where TId : class
{
    /// <summary>True when a new entity must be created for the source one.</summary>
    public bool CreateNew { get; }

    /// <summary>Id of the existing Freem entity the source one is mapped to, if any.</summary>
    public TId? ExistingId { get; }

    public ImportResolutionResponse(bool createNew, TId? existingId)
    {
        CreateNew = createNew;
        ExistingId = existingId;
    }
}
