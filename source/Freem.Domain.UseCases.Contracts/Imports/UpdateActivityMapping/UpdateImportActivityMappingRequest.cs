using Freem.Domain.Activities;
using Freem.Domain.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Imports.UpdateActivityMapping;

public sealed class UpdateImportActivityMappingRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.UpdateActivityMapping;

    public ImportDocumentId DocumentId { get; }
    public ImportActivityId ImportActivityId { get; }

    /// <summary>
    /// Existing activity of the user; null removes the mapping.
    /// </summary>
    public ActivityId? ActivityId { get; }

    public UpdateImportActivityMappingRequest(
        ImportDocumentId documentId,
        ImportActivityId importActivityId,
        ActivityId? activityId)
    {
        ArgumentNullException.ThrowIfNull(documentId);
        ArgumentNullException.ThrowIfNull(importActivityId);

        DocumentId = documentId;
        ImportActivityId = importActivityId;
        ActivityId = activityId;
    }
}
