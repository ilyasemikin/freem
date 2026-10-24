using Freem.Domain.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Imports.Get;

public sealed class GetImportRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.Get;

    public ImportDocumentId Id { get; }

    public GetImportRequest(ImportDocumentId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
