using Freem.Api.Public.Contracts.V1.Imports;
using Freem.Domain.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Imports;

public sealed class ApiListImportActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.ListActivities;

    public ImportDocumentId Id { get; }
    public ImportActivityListRequest Query { get; }

    public ApiListImportActivityRequest(ImportDocumentId id, ImportActivityListRequest query)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(query);

        Id = id;
        Query = query;
    }
}
