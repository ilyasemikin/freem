using Freem.Domain.DataTransfer.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.DataTransfer.Import.Remove;

public sealed class RemoveImportRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Import.Remove;

    public ImportDocumentId Id { get; }

    public RemoveImportRequest(ImportDocumentId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
