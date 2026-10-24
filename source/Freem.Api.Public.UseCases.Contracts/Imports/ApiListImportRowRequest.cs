using Freem.Api.Public.Contracts.V1.Imports;
using Freem.Domain.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Imports;

public sealed class ApiListImportRowRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.ListRows;

    public ImportDocumentId Id { get; }
    public ImportRowListRequest Query { get; }

    public ApiListImportRowRequest(ImportDocumentId id, ImportRowListRequest query)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(query);

        Id = id;
        Query = query;
    }
}
