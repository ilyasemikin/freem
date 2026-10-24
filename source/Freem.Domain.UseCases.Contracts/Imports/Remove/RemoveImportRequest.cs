using Freem.Domain.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Imports.Remove;

public sealed class RemoveImportRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.Remove;

    public ImportDocumentId Id { get; }

    public RemoveImportRequest(ImportDocumentId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
