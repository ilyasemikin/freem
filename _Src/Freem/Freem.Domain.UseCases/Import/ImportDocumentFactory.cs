using Freem.Domain.Entities.Import;
using Freem.Domain.Formats;
using Freem.Domain.Import;
using Freem.Domain.Users;
using Freem.Domain.Users.Models;

namespace Freem.Domain.UseCases.Import;

public static class ImportDocumentFactory
{
    public static ImportDocument CreateDocument(
        ImportParseResult result,
        UserId userId,
        FormatKey format,
        ImportFileName fileName,
        TimeZoneOffset timeZoneOffset)
    {
        ArgumentNullException.ThrowIfNull(result);

        var id = ImportDocumentId.Generate();
        var tags = result.Tags
            .Select(tag => new ImportDocumentTag(tag))
            .ToArray();
        var activities = result.Activities
            .Select(activity => new ImportDocumentActivity(activity))
            .ToArray();
        return new ImportDocument(
            id,
            userId,
            format,
            fileName,
            timeZoneOffset,
            ImportDocumentStatus.Parsed,
            result.RowsInFile,
            result.Records.Count,
            tags,
            activities);
    }

    public static IReadOnlyList<ImportRow> CreateRows(ImportDocumentId documentId, ImportParseResult result)
    {
        ArgumentNullException.ThrowIfNull(documentId);
        ArgumentNullException.ThrowIfNull(result);

        return result.Records
            .Select(record => new ImportRow(ImportRowId.Generate(), documentId, record))
            .ToArray();
    }
}
