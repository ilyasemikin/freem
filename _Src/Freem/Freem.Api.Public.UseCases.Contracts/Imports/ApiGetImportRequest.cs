using Freem.Domain.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Imports;

public sealed class ApiGetImportRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.Get;

    public ImportDocumentId Id { get; }

    public ApiGetImportRequest(ImportDocumentId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
