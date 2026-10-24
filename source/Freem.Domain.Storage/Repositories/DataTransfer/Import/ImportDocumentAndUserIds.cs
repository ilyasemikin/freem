using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Users;

namespace Freem.Domain.Storage.Repositories.DataTransfer.Import;

public sealed class ImportDocumentAndUserIds
{
    public ImportDocumentId DocumentId { get; }
    public UserId UserId { get; }

    public ImportDocumentAndUserIds(ImportDocumentId documentId, UserId userId)
    {
        ArgumentNullException.ThrowIfNull(documentId);
        ArgumentNullException.ThrowIfNull(userId);

        DocumentId = documentId;
        UserId = userId;
    }
}
