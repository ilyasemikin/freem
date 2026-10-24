using Freem.Domain.DataTransfer.Formats;
using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Users.Models;

namespace Freem.Api.Public.Contracts.V1.DataTransfer.Import.Dtos;

/// <summary>An import: an uploaded and parsed file whose rows can be reviewed. Its activities and tags are listed by separate endpoints.</summary>
public sealed class ImportDto
{
    /// <summary>Import id.</summary>
    public ImportDocumentId Id { get; }

    /// <summary>Key of the format the file was parsed with.</summary>
    public FormatKey Format { get; }

    /// <summary>Name of the uploaded file.</summary>
    public ImportFileName FileName { get; }

    /// <summary>Time zone offset used to read the file times.</summary>
    public TimeZoneOffset TimeZoneOffset { get; }

    /// <summary>Import status.</summary>
    public ImportDocumentStatus Status { get; }

    /// <summary>Number of rows in the uploaded file.</summary>
    public int RowsInFile { get; }

    /// <summary>Number of rows that were parsed and saved.</summary>
    public int RowsParsed { get; }

    public ImportDto(
        ImportDocumentId id,
        FormatKey format,
        ImportFileName fileName,
        TimeZoneOffset timeZoneOffset,
        ImportDocumentStatus status,
        int rowsInFile,
        int rowsParsed)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(format);
        ArgumentNullException.ThrowIfNull(fileName);
        ArgumentNullException.ThrowIfNull(timeZoneOffset);
        ArgumentNullException.ThrowIfNull(status);

        Id = id;
        Format = format;
        FileName = fileName;
        TimeZoneOffset = timeZoneOffset;
        Status = status;
        RowsInFile = rowsInFile;
        RowsParsed = rowsParsed;
    }
}
