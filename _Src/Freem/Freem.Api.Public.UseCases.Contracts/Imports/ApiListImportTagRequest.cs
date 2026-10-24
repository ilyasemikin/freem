using Freem.Api.Public.Contracts.V1.Imports;
using Freem.Domain.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Imports;

public sealed class ApiListImportTagRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.ListTags;

    public ImportDocumentId Id { get; }
    public ImportTagListRequest Query { get; }

    public ApiListImportTagRequest(ImportDocumentId id, ImportTagListRequest query)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(query);

        Id = id;
        Query = query;
    }
}
