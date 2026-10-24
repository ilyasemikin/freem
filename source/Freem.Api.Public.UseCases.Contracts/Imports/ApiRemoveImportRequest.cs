using Freem.Domain.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Imports;

public sealed class ApiRemoveImportRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.Remove;

    public ImportDocumentId Id { get; }

    public ApiRemoveImportRequest(ImportDocumentId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
