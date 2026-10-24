using Freem.Domain.DataTransfer.Formats;
using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Users;
using Freem.Domain.Users.Models;

namespace Freem.Domain.Entities.DataTransfer.Import;

public sealed class ImportDocument :
    IEntity<ImportDocumentId>
{
    public ImportDocumentId Id { get; }
    public UserId UserId { get; }
    public FormatKey Format { get; }
    public ImportFileName FileName { get; }
    public TimeZoneOffset TimeZoneOffset { get; }
    public ImportDocumentStatus Status { get; }
    public int RowsInFile { get; }
    public int RowsParsed { get; }

    public ImportDocument(
        ImportDocumentId id,
        UserId userId,
        FormatKey format,
        ImportFileName fileName,
        TimeZoneOffset timeZoneOffset,
        ImportDocumentStatus status,
        int rowsInFile,
        int rowsParsed)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(format);
        ArgumentNullException.ThrowIfNull(fileName);
        ArgumentNullException.ThrowIfNull(timeZoneOffset);
        ArgumentNullException.ThrowIfNull(status);
        ArgumentOutOfRangeException.ThrowIfNegative(rowsInFile);
        ArgumentOutOfRangeException.ThrowIfNegative(rowsParsed);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(rowsParsed, rowsInFile);

        Id = id;
        UserId = userId;
        Format = format;
        FileName = fileName;
        TimeZoneOffset = timeZoneOffset;
        Status = status;
        RowsInFile = rowsInFile;
        RowsParsed = rowsParsed;
    }
}
