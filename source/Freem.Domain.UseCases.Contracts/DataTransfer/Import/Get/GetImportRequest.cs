using Freem.Domain.DataTransfer.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.DataTransfer.Import.Get;

public sealed class GetImportRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Import.Get;

    public ImportDocumentId Id { get; }

    public GetImportRequest(ImportDocumentId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
